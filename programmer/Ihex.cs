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

        public async Task<List<Section>> parse(IProgress<int> progress)
        {
            List<Section> sections = new List<Section>();
            bool eof_flag = false;
            int sr_len = 0;
            using (var sr = new StreamReader(this.filename, Encoding.ASCII))
            {
                while (await sr.ReadLineAsync() != null)
                {
                    sr_len++;
                }
            }
            int sr_idx = 0;
            using (var sr = new StreamReader(this.filename, Encoding.ASCII))
            {
                string line;
                int s_i = 0;//sections index
                UInt16 ext_addr = 0;

                while ((line = await sr.ReadLineAsync()) != null)
                {
                    if (line.Length < 10)
                        throw new IhexIndexFormatError(filename);
                    if (line[0] != ':')
                        throw new IhexFormatError(filename);
                    UInt16 reclen = UInt16.Parse(line.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                    UInt32 address = UInt32.Parse(line.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
                    UInt16 content_type = UInt16.Parse(line.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
                    UInt16 chksum = UInt16.Parse(line.Substring(line.Length - 2), System.Globalization.NumberStyles.HexNumber);
                    UInt32 sum = reclen + (address >> 8) + (address & byte.MaxValue) + content_type + chksum;
                    byte[] data = new byte[reclen];
                    // check record length and aprse data
                    if (reclen != 0)
                    {
                        if (line.Length == 11 + reclen * 2)
                        {
                            for (int i = 0; i < reclen; i++)
                            {
                                var s = line.Substring(9 + i * 2, 2);

                                data[i] = Convert.ToByte(s, 16);//byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                                sum += data[i];
                            }
                            if ((sum & byte.MaxValue) != 0)
                                throw new IhexFormatError(filename);
                        }
                        else
                            throw new IhexFormatError(filename);
                    }
                    else
                        data = new byte[0];

                    progress.Report(sr_idx++ * 100 / sr_len);
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
            if (eof_flag == false)
                throw new IhexFormatError(filename);
            return sections;
        }
        public List<Section> parse()
        {
            List<Section> sections = new List<Section>();
            bool eof_flag = false;
            int sr_len = 0;
            using (var sr = new StreamReader(this.filename, Encoding.ASCII))
            {
                while (sr.ReadLine() != null)
                {
                    sr_len++;
                }
            }
            int sr_idx = 0;
            using (var sr = new StreamReader(this.filename, Encoding.ASCII))
            {
                string line;
                int s_i = 0;//sections index
                ushort ext_addr = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length < 10)
                        throw new IhexIndexFormatError(filename);
                    if (line[0] != ':')
                        throw new IhexFormatError(filename);
                    UInt16 reclen = UInt16.Parse(line.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                    UInt32 address = UInt32.Parse(line.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
                    UInt16 content_type = UInt16.Parse(line.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
                    UInt16 chksum = UInt16.Parse(line.Substring(line.Length - 2), System.Globalization.NumberStyles.HexNumber);
                    UInt32 sum = reclen + (address >> 8) + (address & byte.MaxValue) + content_type + chksum;
                    byte[] data = new byte[reclen];
                    // check record length and aprse data
                    if (reclen != 0)
                    {
                        if (line.Length == 11 + reclen * 2)
                        {
                            for (int i = 0; i < reclen; i++)
                            {
                                var s = line.Substring(9 + i * 2, 2);

                                data[i] = Convert.ToByte(s, 16);//byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                                sum += data[i];
                            }
                            if ((sum & byte.MaxValue) != 0)
                                throw new IhexFormatError(filename);
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
                            sections.Add(new Section { address = (uint)(ext_addr << 16) + address, data = data });
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
            if (eof_flag == false)
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

        /// <summary>
        /// Padding each data block with `space_data` to let block size fit pgsz * N.
        /// </summary>
        /// <param name="sections">response from `parse`</param>
        /// <param name="page_size">page size, e.g. 256, 512.</param>
        /// <param name="space_data">the byte data used to padding. e.g. 0xff</param>
        /// <returns></returns>
        public static List<Section> padding_space(List<Section> sections, uint page_size, byte space_data)
        {
            var new_section = sections.Select(sect =>
             {
                 var sect_addr = sect.address;
                 var sect_data = sect.data;

                 // 起始位置若不是在 page_size * N 上
                 // 往前補 0XFF
                 if (sect_addr % page_size != 0)
                 {
                     uint n = sect_addr / page_size;
                     uint l = sect_addr - page_size * n;
                     sect_addr = page_size * n;
                     var ff = Enumerable.Repeat((byte)0xFF, (int)l).ToArray();
                     sect_data = ff.Concat(sect_data).ToArray();
                 }

                 // 結束位置 +1 若不是在 page_size * N 上
                 // 往後補 0XFF
                 if ((sect_addr + sect_data.Length) % page_size != 0)
                 {
                     uint n = (sect_addr + (uint)sect_data.Length) / page_size;
                     uint l = page_size * (n + 1) - (sect_addr + (uint)sect_data.Length);
                     var ff = Enumerable.Repeat((byte)0xff, (int)l).ToArray();
                     sect_data = sect_data.Concat(ff).ToArray();
                 }
                 var new_sect = new Section { address = sect_addr, data = sect_data };
                 return new_sect;
             });
            return new_section.ToList();
        }
        /// <summary>
        /// Cut each data block to pages.
        /// </summary>
        /// <param name="sections">response from `padding_space`.</param>
        /// <param name="page_size">page size, e.g. 256, 512.</param>
        /// <returns></returns>
        public static List<Section> cut_to_pages(List<Section> sections, int page_size)
        {
            List<Section> pages = new List<Section>();

            foreach (var section in sections)
            {
                var sect_addr = section.address;
                var sect_data = section.data;
                int page_len = sect_data.Length / page_size;
                pages.Capacity+= page_len;
                for (uint i = 0; i < page_len; i++)
                {
                    pages.Add(new Section
                    {
                        address = sect_addr + i * (uint)page_size,
                        data = sect_data.ToList().GetRange((int)i * page_size, page_size).ToArray()
                    });
                }

            }
            return pages;

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
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Section p = (Section)obj;
                return (this.address == p.address) && (this.data.SequenceEqual(p.data));
            }
        }
    }
    public class IhexIndexFormatError : Exception
    {
        string filename;
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


