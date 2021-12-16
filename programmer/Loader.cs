using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
namespace programmer
{
    
    class Loader
    {
        int device_type;
        string device_name;

        bool is_flash_prog;
        //bool is_ext_flash_prog;
        //bool is_ext_to_int;
        //bool is_eeprom_prog;
        bool is_go_app;

        string flash_file;
        //string ext_flash_file;
        //string eep_file;
        UInt16 go_app_delay;
        SerialPort port;
        // 燒錄狀態
        enum Stage : int
        {
            PREPARE = 0,
            FLASH_PROG = 1,
            EEP_PROG = 2,
            EXT_FLASH_PROG = 3,
            EXT_TO_INT = 4,
            END = 5
        }

        Stage stage = Stage.PREPARE;
        int totle_step;
        int cur_step;
        List<byte> flash_pages = new List<byte>();
        int flash_page_idx;

        int flash_size = 0;
        float prog_time;

        Loader(SerialPort serial, int device_type = 0, bool is_flash_prog = false, bool is_go_app = false, string flash_file = "", UInt16 go_app_delay = 0)
        {
            this.port = serial;
            this.device_type = device_type;
            this.is_flash_prog = is_flash_prog;
            this.is_go_app = is_go_app;
            this.flash_file = flash_file;
            this.go_app_delay = go_app_delay;


        }

        void prepare()
        {
            if (this.device_type > Serial_Info.TotolNum)
                throw new DeviceTypeError(this.device_type.ToString());
            if (this.is_flash_prog)
                if (!File.Exists(this.flash_file))
                    throw new FileNotFoundException();
            if (this.is_go_app)
                if (go_app_delay > 65535)
                    throw new GoAppDelayValueError(this.go_app_delay);
            prepare_flash();
            prepare_device();
        }
        async void prepare_flash()
        {
            if(this.is_flash_prog)
            {
                Ihex ihex = new Ihex(this.flash_file);
                try
                {
                    var blocks= await ihex.parse();
                    this.flash_size = blocks.Select(x => x.data.Length).Sum();
                    
                }
                catch(Exception e)
                {
                    throw new FlashIsNotIhexError(this.flash_file);
                }
            }
        }
        void prepare_device()
        {

        }
    }
    public class LoaderException : Exception
    {
        public LoaderException() : base() { }
        public LoaderException(string message) : base(message) { }
        public LoaderException(string message, Exception e) : base(message, e) { }
    }
    public class DeviceTypeError : Exception
    {
        public DeviceTypeError() : base() { }
        public DeviceTypeError(string message) : base(message) { }
        public DeviceTypeError(string message, Exception e) : base(message, e) { }
    }
    public class GoAppDelayValueError:Exception
    {
        public GoAppDelayValueError() : base(){ }
        public GoAppDelayValueError(int message) : base($"Value { message} is over 65535") { }
        public GoAppDelayValueError(int message,Exception e) : base($"Value { message} is over 65535", e) { }
    }
}
