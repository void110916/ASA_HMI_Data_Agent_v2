using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Management;
using Microsoft.Win32;
using Microsoft.CodeAnalysis;

namespace programmer
{
    class Program
    {

        public static Dictionary<string, dynamic>[] device_list = new Dictionary<string, dynamic>[]
        {
            new Dictionary<string,dynamic>
            {
                ["name"]="auto",
                ["dev_type"]=0,
                ["protocol_version"]=0,
                ["userapp_start"]=0,
                ["userapp_size"]=0,
                ["note"]="預設值，自動檢測裝置類型。"
            },
            new Dictionary<string,dynamic>
            {
                ["name"]="asa_m128_v1",
                ["dev_type"]=1,
                ["protocol_version"]=1,
                ["userapp_start"]=0x_0000_0000,
                ["userapp_size"]=0x0001_F000,
                ["note"]=""
            },
            new Dictionary<string,dynamic>
            {
                ["name"]="asa_m128_v2",
                ["dev_type"]=2,
                ["protocol_version"]=1,
                ["userapp_start"]=0x_0000_0000,
                ["userapp_size"]=0x0001_F000,
                ["note"]=""
            },
            new Dictionary<string,dynamic>
            {
                ["name"]="asa_m128_v3",
                ["dev_type"]=3,
                ["protocol_version"]=2,
                ["userapp_start"]=0x_0000_0000,
                ["userapp_size"]=0x0001_F000,
                ["note"]=""
            },
            new Dictionary<string,dynamic>
            {
                ["name"]="asa_m3_v1",
                ["dev_type"]=4,
                ["protocol_version"]=2,
                ["userapp_start"]=0x_0000_1000,
                ["userapp_size"]=0x0001_F000,
                ["note"]=""
            },
            new Dictionary<string,dynamic>
            {
                ["name"]="asa_m4_v1",
                ["dev_type"]=5,
                ["protocol_version"]=2,
                ["userapp_start"]=0x_0001_0000,
                ["userapp_size"]=0x000F_0000,
                ["note"]=""
            }
         };
        static string[] dynBar = { "-", "/", "|", @"\" };
        static Loader loader;
        static async Task<int> Main(string[] args)
        {
            Argparse parser = new Argparse();
            try
            {
                parser = new Argparse(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
#if DEBUG
                Console.ReadKey();
#endif
                Environment.Exit(1);
            }

            switch (parser.work)
            {
                case Work.print_ports:
                    foreach (var port in Serial_Info.Search())
                    {

                        Console.WriteLine("Port Name:    " + port.PortName);
                        Console.WriteLine("Description:  " + port.Caption);
                        Console.WriteLine("Manufacturer: " + port.Manufact);
                        Console.WriteLine("Device ID:    " + port.DeviceID);
                        Console.WriteLine("-----------------------------------");
                    }
                    break;
                case Work.print_devices:
                    Console.WriteLine("可用裝置種類如下：");
                    Console.WriteLine($"    {"裝置名稱",-16}{"編號",-6}{"備註"}");
                    foreach (var dev in device_list)
                    {
                        Console.WriteLine($"  - {dev["name"],-20}{dev["dev_type"],-8}{dev["note"]}");
                    }
                    break;
                case Work.program:
                    var idx = 0;
                    var progress = new Progress<int>(prog =>
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write(new string(' ', Console.BufferWidth));
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write($"{prog,3}% {dynBar[idx++ % 4]}");
                    });
                    await programming(parser, progress);
                    break;
                case Work.help:
                    print_help();
                    break;
            }

#if DEBUG
            Console.ReadKey();
#endif

            return 0;
        }
        static void print_help()
        {
            Console.Write("usage: asaloader [-h] [--lc LC] {prog,print-devices,pd,print-ports,pp} ...\r\n" +
                "\r\n" +
                "燒錄程式到ASA系列開發版。\r\n" +
                "\r\n" +
                "positional arguments:\r\n" +
                "\tprog                燒入程式進入開發版。\r\n" +
                "\tprint-devices (pd)  列出可用裝置。\r\n" +
                "\tprint-ports (pp)    列出目前電腦上所有的串列埠。\r\n" +
                "\r\n" +
                "optional arguments:\r\n" +
                "  -h, --help            show this help message and exit\r\n"); //+
                                                                                //"--lc LC               設置語言代碼。例如\"zh_TW\",\"en_US\"");
        }
        static void programming(string hex_file, SerialPort port, bool is_go_delay, int go_app_delay = 0)
        {




        }
        static async Task programming(Argparse args, IProgress<int> progress)
        {
            SerialPort port = new SerialPort(args.port, 115200);
            try
            {
                port.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
#if DEBUG
                Console.ReadKey();
#endif
                Environment.Exit(1);
            }

            bool is_flash_prog = false;
            // 是否有內部 flash 要被燒錄
            if (args.flash_file == null)
                is_flash_prog = false;
            else
                is_flash_prog = true;

            //Serial_Info.Search();
            int device_type = device_list[args.device]["dev_type"];
            var loader = new Loader(port, device_type: device_type, is_flash_prog: is_flash_prog, is_go_app: args.is_go_app,
                                                             flash_file: args.flash_file, go_app_delay: args.go_app_delay);
            try
            {
                await loader.prepare(progress);
                await loader.loading(progress);
            }
            catch (ComuError)
            {
                Console.WriteLine("ERROR: Can't communicate with the device.");
                Console.WriteLine("       Please check the comport and the device.");
#if DEBUG
                Console.ReadKey();
#endif
                Environment.Exit(1);
            }
            catch (CheckDeviceError e)
            {

                Console.WriteLine("ERROR: Device is not match.");
                Console.WriteLine($"       Assigned device is {device_list[e.in_dev]["name"]}");
                Console.WriteLine($"       Detected device is {device_list[e.real_dev]["name"]}");
#if DEBUG
                Console.ReadKey();
#endif
                Environment.Exit(1);
            }

        }
    }
    enum Work
    {
        print_ports,
        print_devices,
        program,
        help
    }
    class Argparse
    {
        public Work work;
        public string flash_file;
        public int device;
        public string port;
        public bool is_go_app = false;
        public UInt16 go_app_delay = 5000;
        public Argparse() { }
        public Argparse(string[] arg)
        {
            this.work = Work.help;
            if (arg.Length == 0)
            {
                return;
            }
            switch (arg[0])
            {
                case "pp":
                case "print-ports":
                    this.work = Work.print_ports;
                    break;
                case "pd":
                case "print-devices":
                    this.work = Work.print_devices;
                    break;
                case "prog":
                    this.work = Work.program;
                    break;
            }
            if (this.work == Work.program)
            {
                for (int i = 1; i < arg.Length; i++)
                {
                    switch (arg[i])
                    {
                        case "-d":
                        case "--device":
                            if (int.TryParse(arg[++i], out this.device))
                                throw new ArgumentException($"Value {arg[i]} is not usable", "device");
                            break;
                        case "-p":
                        case "--port":
                            this.port = arg[++i];
                            break;
                        case "-f":
                        case "--flash":
                            var file = arg[++i];
                            if (File.Exists(file) && file.EndsWith(".hex"))
                                this.flash_file = file;
                            else
                                throw new FileNotFoundException($"Cannot find file {file}");
                            break;
                        case "-a":
                        case "--after-prog-go-app":
                            this.is_go_app = true;
                            break;
                        case "-D":
                        case "--go-app-delay":
                            if (!UInt16.TryParse(arg[++i], out this.go_app_delay))
                                throw new ArgumentException($"Value {arg[i]} is not usable", "go-app-delay");
                            break;
                    }
                }
            }
            else
                return;

        }
    }

}
