using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Threading;
namespace programmer
{

    public class Loader
    {
        public int device_type;
        public string device_name;

        public bool is_flash_prog;
        //bool is_ext_flash_prog;
        //bool is_ext_to_int;
        //bool is_eeprom_prog;
        public bool is_go_app;

        public string flash_file;
        //string ext_flash_file;
        //string eep_file;
        public UInt16 go_app_delay;
        SerialPort port;
        CMD cth;
        // 燒錄狀態
        public enum Stage : int
        {
            PREPARE = 0,
            FLASH_PROG = 1,
            //EEP_PROG = 2,
            //EXT_FLASH_PROG = 3,
            //EXT_TO_INT = 4,
            END = 2
        }

        List<Stage> stage;
        int totle_step;
        int cur_step;
        List<Section> flash_pages = new List<Section>();
        int flash_page_idx;

        int flash_size = 0;
        float prog_time;
        int protocol_version;
        public Loader(SerialPort serial, int device_type = 0, bool is_flash_prog = false, bool is_go_app = false, string flash_file = "", UInt16 go_app_delay = 0)
        {
            this.port = serial;
            this.device_type = device_type;
            this.is_flash_prog = is_flash_prog;
            this.is_go_app = is_go_app;
            this.flash_file = flash_file;
            this.go_app_delay = go_app_delay;
            this.cth = new CMD(serial);

        }
        public Loader(SerialPort serial)
        {
            this.port = serial;
        }
        /// <summary>
        /// 燒錄前準備函式
        /// 
        /// 1. 檢查參數  
        /// 2. 檢查flash、eeprom燒錄檔案  
        /// 3. 偵測裝置  
        /// 4. 生成動作列表
        /// </summary>
        public async Task prepare(IProgress<int> progress)
        {
            if (this.device_type > Program.device_list.Length)
                throw new DeviceTypeError(this.device_type.ToString());
            if (this.is_flash_prog)
                if (!File.Exists(this.flash_file))
                    throw new FileNotFoundException();
            if (this.is_go_app)
                if (go_app_delay > UInt16.MaxValue)
                    throw new GoAppDelayValueError(this.go_app_delay);
            prepare_device();
            await prepare_flash(progress);
            progress.Report(10);



            // stage
            this.stage = new List<Stage>();
            if (this.is_flash_prog)
            {
                this.stage.Add(Stage.FLASH_PROG);
                this.totle_step += this.flash_pages.Count;
            }
            this.stage.Add(Stage.END);
            this.totle_step++;

            // prog time
            switch (this.device_type)
            {
                case 1:
                case 2:
                    this.prog_time = flash_pages.Count * 0.047f + 0.23f;
                    break;
                case 3:
                    // asa_m128_v2
                    this.prog_time = flash_pages.Count * 0.047f + 0.23f;
                    break;
                case 4:
                    //asa_m3_v1
                    this.prog_time = flash_pages.Count * 0.5f + 0.23f;
                    break;
                case 5:
                    //asa_m4_v1
                    this.prog_time = flash_pages.Count * 0.5f + 0.23f + 3;
                    break;
            }
            Console.Write($"預計燒錄時間: {this.prog_time} s");

        }

        /// <summary>
        /// 處理flash燒錄檔
        ///
        ///Raises:
        ///    exceptions.FlashIsNotIhexError: flash 燒錄檔非 intel hex 格式
        ///
        ///處理事務：
        ///    1. 偵測是否為 intel hex 格式
        ///    2. 取出資料
        ///    3. padding_space
        ///    4. cut_to_pages
        /// </summary>
        async Task prepare_flash(IProgress<int> progress)
        {
            if (this.is_flash_prog)
            {
                Ihex ihex = new Ihex(this.flash_file);
                try
                {
                    var blocks = await ihex.parse(progress);
                    this.flash_size = blocks.Select(x => x.data.Length).Sum();
                    blocks = Ihex.padding_space(blocks, 256, 0xff);
                    this.flash_pages = Ihex.cut_to_pages(blocks, 256);
                }
                catch (Exception e)
                {
                    throw new FlashIsNotIhexError(this.flash_file, e);
                }
            }
        }
        /// <summary>
        /// 檢查裝置是否吻合設定的裝置號碼
        /// Raises:
        ///     exceptions.ComuError: 無法通訊
        ///     exceptions.CheckDeviceError: 裝置比對錯誤
        /// </summary>
        void prepare_device()
        {
            int version = 0;
            CMD cmd = new CMD(port);
            var res = cmd.chk_protocal(out version);
            int detected_device = 0;
            if (res && version == 1)
            {
                // protocol v1 dosn't have "chk_device" command
                // m128_v1 or m128_v2
                // use m128_v2 for default
                detected_device = 2;
            }
            else if (res && version == 2)
            {
                res = cmd.v2_prog_chk_device(out detected_device);
                if (!res)
                    throw new ComuError();
            }
            else
                throw new ComuError();

            // auto detect device
            if (Program.device_list[device_type]["protocol_version"] == 0)
                this.device_type = detected_device;

            // check for protocol v1 (e.g. m128_v1, m128_v2)
            else if (Program.device_list[device_type]["protocol_version"] == 1)
            {
                if (this.device_type != 2 || this.device_type != 1)
                    throw new CheckDeviceError(this.device_type, detected_device);
            }

            // check for protocol v2 (e.g. m128_v3, m3_v1)
            else if (Program.device_list[device_type]["protocol_version"] == 2)
            {
                if (this.device_type != detected_device)
                    throw new CheckDeviceError(this.device_type, detected_device);
            }

            this.protocol_version = Program.device_list[device_type]["protocol_version"];
            this.device_name = Program.device_list[device_type]["name"];
        }
        public async Task loading(IProgress<int> progress)
        {
            foreach (var st in this.stage)
            {
                if (st == Stage.FLASH_PROG)
                    await Task.Run(() => prog_loading(progress));
                else if (st == Stage.END)
                    await Task.Run(() => prog_end(progress));
            }

        }
        void prog_loading(IProgress<int> progress)
        {
            var flash_page_total = this.flash_pages.Count;
            for (; this.flash_page_idx < flash_page_total; this.flash_page_idx++, this.cur_step++)
            {
                var address = this.flash_pages[this.flash_page_idx].address;
                var data = this.flash_pages[this.flash_page_idx].data;
                if (this.protocol_version == 1)
                {
                    // protocol v1 will auto clear flash after command "chk_protocol"
                    this.cth.v1_prog_flash_wr(data);
                    Thread.Sleep(30);
                }
                else if (this.protocol_version == 2)
                {
                    if (this.flash_page_idx == 0)
                    {
                        if (this.device_type == 5)
                        {
                            // asa_m4_v1 takes longer chip erase time
                            this.cth.v3_flash_erase_all();
                        }
                        else
                            this.cth.v2_flash_erase_all();
                    }
                    this.cth.v2_flash_wr(address, data);
                }
                progress.Report(10 + this.flash_page_idx * 90 / flash_page_total);
            }


        }

        void prog_end(IProgress<int> progress)
        {
            if (this.protocol_version == 1)
                this.cth.v1_prog_end();
            else if (this.protocol_version == 2)
            {
                if (this.is_go_app)
                {
                    this.cth.v2_prog_set_go_app_delay(this.go_app_delay);
                    this.cth.v2_prog_end_and_go_app();
                }
                else
                    this.cth.v2_prog_end();
            }
            progress.Report(100);
        }
    }
    public class CMD
    {
        SerialPort port;
        public readonly static Encoding encoder = Encoding.GetEncoding(437);
        readonly byte[] HEADER = new byte[] { 0xfc, 0xfc, 0xfc };
        readonly byte TOCKEN = 0x01;
        public class COMMAND
        {
            public CommanderHeader command;
            public byte[] data;
            public COMMAND(CommanderHeader command, byte[] data)
            {
                this.command = command;
                this.data = data;
            }
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                COMMAND p = (COMMAND)obj;
                if (this.command != p.command)
                    return false;

                return this.data.SequenceEqual(p.data);
            }
        }
        public CMD()
        {

        }
        public CMD(SerialPort port)
        {
            this.port = port;
            this.port.Encoding = encoder;
            if (!this.port.IsOpen)
                this.port.Open();
            this.port.DiscardInBuffer();
        }

        public bool chk_protocal(out int version)
        {
            version = 0;
            put_packet(CommanderHeader.CHK_PROTOCOL, encoder.GetBytes("test"));
            //Thread.Sleep(500);
            var rep = get_packet();

            if (rep == null)
                return false;
            if ((rep.command == CommanderHeader.ACK1) && (encoder.GetString(rep.data) == "OK!!"))
            {
                version = 1;
                return true;
            }
            else if ((rep.command == CommanderHeader.CHK_PROTOCOL) && (rep.data[0] == 0))
            {
                version = rep.data[1];
                return true;
            }
            else
                return false;
        }
        public bool v1_prog_flash_wr(byte[] data)
        {
            return this.put_packet(CommanderHeader.DATA, data);

        }
        public bool v2_prog_chk_device(out int version)
        {
            version = 0;
            put_packet(CommanderHeader.PROG_CHK_DEVICE, new byte[] { });

            var rep = get_packet();
            if (rep.command == CommanderHeader.PROG_CHK_DEVICE && rep.data[0] == 0)
            {
                version = rep.data[1];
                return true;
            }
            else
                return false;

        }

        public bool v2_flash_erase_all()
        {
            this.put_packet(CommanderHeader.FLASH_EARSE_ALL, new byte[] { });
            Thread.Sleep(2200);
            var rep = this.get_packet();
            if (rep.command == CommanderHeader.FLASH_EARSE_ALL && rep.data[0] == 0)
                return true;
            else
                return false;
        }

        public bool v3_flash_erase_all()
        {
            this.put_packet(CommanderHeader.FLASH_EARSE_ALL, new byte[] { });
            Thread.Sleep(3000);
            var rep = this.get_packet();
            if (rep.command == CommanderHeader.FLASH_EARSE_ALL && rep.data[0] == 0)
                return true;
            else
                return false;
        }

        public bool v2_flash_wr(uint page_addr, byte[] data)
        {
            var loadlist = new List<byte>(sizeof(uint) / sizeof(byte) + data.Length);
            loadlist.AddRange(BitConverter.GetBytes(page_addr));
            loadlist.AddRange(data);
            var payload = loadlist.ToArray();
            this.put_packet(CommanderHeader.FLASH_WRITE, payload);
            var rep = this.get_packet();
            if (rep.command == CommanderHeader.FLASH_WRITE && rep.data[0] == 0)
                return true;
            else
                return false;
        }

        public bool v1_prog_end()
        {
            this.put_packet(CommanderHeader.DATA, new byte[] { });
            var rep = get_packet();
            if (rep.command == CommanderHeader.ACK2 && encoder.GetString(rep.data) == "OK!!")
                return true;
            else
                return false;
        }

        public bool v2_prog_end()
        {
            this.put_packet(CommanderHeader.PROG_END, new byte[] { });
            var rep = get_packet();
            if (rep.command == CommanderHeader.PROG_END && rep.data[0] == 0)
                return true;
            else
                return false;
        }
        public bool v2_prog_set_go_app_delay(UInt16 delay)
        {
            this.put_packet(CommanderHeader.PROG_SET_GO_APP_DELAY, new byte[] { });
            var rep = get_packet();
            if (rep.command == CommanderHeader.PROG_SET_GO_APP_DELAY && rep.data[0] == 0)
                return true;
            else
                return false;
        }
        public bool v2_prog_end_and_go_app()
        {
            this.put_packet(CommanderHeader.PROG_END_AND_GO_APP, new byte[] { });
            var rep = get_packet();
            if (rep.command == CommanderHeader.PROG_END_AND_GO_APP && rep.data[0] == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 發送封包函式(polling)
        /// </summary>
        /// <param name="command">命令編號</param>
        /// <param name="data">封包資料</param>
        /// <returns></returns>
        bool put_packet(CommanderHeader command, byte[] data)
        {
            if (this.port.IsOpen)
            {
                //this.port.DiscardInBuffer();
                //this.port.DiscardOutBuffer();
                var pac = encode(command, data);
                this.port.Write(pac, 0, pac.Length);
                return true;
            }
            else
                return false;

        }
        COMMAND get_packet()
        {
            Thread.Sleep(15);
            var ch = port.ReadExisting();
            var packet = decode(ch);
            return packet;
        }


        protected byte[] encode(CommanderHeader command, byte[] data)
        {
            var pac = new List<byte>(data.Length + 8);
            pac.AddRange(HEADER);
            pac.Add((byte)command);
            pac.Add(TOCKEN);
            byte[] data_len = { (byte)(data.Length >> 8), (byte)(data.Length & 255) };
            pac.Add(data_len[0]);
            pac.Add(data_len[1]);
            pac.AddRange(data);
            byte chksum = (byte)(data.Sum(x => x) & 255);
            pac.Add(chksum);
            return pac.ToArray();
        }

        protected COMMAND decode(string ch)
        {
            var pac = encoder.GetBytes(ch);
            COMMAND packet = new COMMAND(CommanderHeader.PROG_END, new byte[] { });
            int pac_len = 0;
            CommanderHeader? command = null;
            if (pac.Length == 0)
                return packet;
            else if (ch.Substring(0, 3) == encoder.GetString(HEADER))
            {
                command = (CommanderHeader)pac[3];
            }

            if (pac[4] != TOCKEN)
                return packet;
            else
            {
                pac_len = (pac[5] << 8) + pac[6];

                byte[] data = encoder.GetBytes(ch.Substring(7, pac_len));
                var sum = data.Sum(x => x);


                if ((sum & 255) != pac[pac.Length - 1])
                    return packet;
                else
                    packet = new COMMAND(command.Value, data);
            }
            return packet;
        }
    }
    public enum CommanderHeader : byte
    {
        // for both version 1 and version 2
        CHK_PROTOCOL = 0xFA,

        // for version 1 protocol supproted device such as
        //  asa_m128_v1
        //  asa_m128_v2
        ACK1 = 0xFB,
        DATA = 0xFC,
        ACK2 = 0xFD,

        // for version 2 protocol supproted device such as
        //  asa_m128_v3
        //  asa_m3_v1
        PROG_CHK_DEVICE = 0x02,
        PROG_END = 0x03,
        PROG_END_AND_GO_APP = 0x04,
        PROG_SET_GO_APP_DELAY = 0x05,


        PROG_EXT_TO_INT = 0x06,

        FLASH_SET_PGSZ = 0x10,
        FLASH_GET_PGSZ = 0x11,
        FLASH_WRITE = 0x12,
        FLASH_READ = 0x13,
        FLASH_VARIFY = 0x14,
        FLASH_EARSE_SECTOR = 0x15,
        FLASH_EARSE_ALL = 0x16

    }
    public class LoaderException : Exception
    {
        public LoaderException() : base() { }
        public LoaderException(string message) : base(message) { }
        public LoaderException(string message, Exception e) : base(message, e) { }
    }
    public class DeviceTypeError : Exception
    {
        string message;
        public DeviceTypeError() : base() { }
        public DeviceTypeError(string message) : base(message) { }
        public DeviceTypeError(string message, Exception e) : base(message, e) { }
    }
    public class GoAppDelayValueError : Exception
    {
        public GoAppDelayValueError() : base() { }
        public GoAppDelayValueError(int message) : base($"Value { message} is over 65535") { }
        public GoAppDelayValueError(int message, Exception e) : base($"Value { message} is over 65535", e) { }
    }
    public class ComuError : Exception
    {
        public ComuError() : base() { }

    }
    public class CheckDeviceError : Exception
    {
        public int in_dev;
        public int real_dev;
        public CheckDeviceError(int type1, int type2) : base($"device type {type1} is not equals to detect device type {type2}")
        {
            this.in_dev = type1;
            this.real_dev = type2;
        }
    }
}
