using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
namespace programmer
{
    class Ihex
    {
        
        string filename;
        public Ihex(string filename)
        {
            this.filename = filename;

        }

        public async Task<List<Section>> parse()
        {
            List<Section> sections = new List<Section>();
            bool eof_flag = false;
            UInt32 sum = 0;
            using (var sr = new StreamReader(this.filename))
            {
                string line;
                int s_i = 0;//sections index
                UInt16 ext_addr = 0;

                while ((line = await sr.ReadLineAsync()) != string.Empty)
                {
                    if (line.Length < 10)
                        throw new IhexIndexFormatError(filename);
                    if (line[0] != ':')
                        throw new IhexFormatError(filename);
                    UInt16 reclen = UInt16.Parse(line.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                    UInt32 address = UInt32.Parse(line.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
                    UInt16 content_type = UInt16.Parse(line.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
                    UInt16 chksum = UInt16.Parse(line.Substring(line.Length - 3), System.Globalization.NumberStyles.HexNumber);
                    sum = reclen + (address >> 16) + address + content_type + chksum;
                    byte[] data = new byte[reclen * 2];
                    // check record length and aprse data
                    if (reclen != 0)
                    {
                        if (line.Length == 12 + reclen * 2)
                        {
                            for (int i = 0; i < reclen; i += 2)
                            {
                                data[i] = byte.Parse(line.Substring(9 + i, 2), System.Globalization.NumberStyles.HexNumber);
                                sum += data[i];
                            }
                        }
                        else
                            throw new IhexFormatError(filename);
                    }
                    else
                        data = new byte[0];
                    if (content_type == 0)
                    {     // data
                        if (s_i == 0)
                        {
                            sections.Add(new Section { address = address, data = data });
                            s_i++;
                        }
                        else if ((ext_addr << 16) + address == sections[s_i - 1].address + sections[s_i - 1].data.Length)
                            sections[s_i - 1] = new Section { address = sections[s_i - 1].address, data = sections[s_i - 1].data.Concat(data).ToArray() };
                        else
                        {
                            sections.Add(new Section { address = address, data = data });
                            s_i++;
                        }
                    }
                    else if (content_type == 1)
                    {
                        // End Of File
                        if (address == 0)
                            eof_flag = true;
                        else
                            throw new IhexFormatError(filename);
                    }
                    else if (content_type == 2)
                        //Extended Segment Address
                        continue;
                    else if (content_type == 3)
                        //Start Segment Address
                        continue;
                    else if (content_type == 4)
                        //Extended Linear Address
                        ext_addr = UInt16.Parse(line.Substring(9, 4), System.Globalization.NumberStyles.HexNumber);
                    else if (content_type == 5)
                        //Start Liner Address
                        continue;

                }
            }
            if (eof_flag == false || ((sum & UInt16.MaxValue) != 0))
                throw new IhexFormatError(filename);
            return sections;
        }
        //public async Task parse()
        //{
        //    string lines;
        //    using (var sr = new StreamReader(this.filename))
        //    {

        //        lines = await sr.ReadToEndAsync();

        //    }
        //    var linelist = lines.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        //    var tasks = linelist.Select<string, Task>((line, index) => lineParse(line, index));
        //    await Task.WhenAll(tasks);
        //}
        //Task lineParse(string line, int index)
        //{
        //    // NOTE line is without "\r\n"
        //    if (line.Length < 10)
        //        throw new IhexIndexFormatError(filename);
        //}
        public List<Section> padding_space(List<Section> sections,int page_size,byte space_data)
        {
            var new_section= sections.Select(sect =>
            {
                var sect_addr = sect.address;
                var sect_data = sect.data;

                // 起始位置若不是在 page_size * N 上
                // 往前補 0XFF
                if(sect_addr%page_size!=0)
                {

                }

                // 結束位置 +1 若不是在 page_size * N 上
                // 往後補 0XFF
                if (sect_addr % page_size != 0)
                {

                }

                return sect;
            });
            return new_section.ToList();
        }
    }
    public struct Section
    {
        public uint address;
        public byte[] data;
        public Section(uint address, byte[] data)
        {
            this.address = address;
            this.data = data;
        }
    }
    public class IhexIndexFormatError : Exception
    {
        public IhexIndexFormatError() : base() { }
        public IhexIndexFormatError(string message) : base(message) { }
        public IhexIndexFormatError(string message, Exception e) : base(message, e) { }
    }
    public class IhexFormatError : Exception
    {
        public IhexFormatError() : base() { }
        public IhexFormatError(string message) : base(message) { }
        public IhexFormatError(string message, Exception e) : base(message, e) { }
    }
    public class FlashIsNotIhexError : Exception
    {
        public FlashIsNotIhexError() : base() { }
        public FlashIsNotIhexError(string message) : base(message) { }
        public FlashIsNotIhexError(string message, Exception e) : base(message, e) { }

    }
}


