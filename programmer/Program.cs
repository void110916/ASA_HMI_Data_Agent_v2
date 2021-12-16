using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Management;
using Microsoft.Win32;

namespace programmer
{
    class Program
    {
        
        static Dictionary<string, dynamic>[] device_list = new Dictionary<string, dynamic>[]
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
        static int Main(string[] args)
        {

            if (args.Length == 0)
            {
                print_help();
                return 0;
            }
            else if (args[0] == "print-ports" || args[0] == "pp")
            {

                foreach (var port in Serial_Info.Search())
                {

                    Console.WriteLine("Port Name:    " + port.PortName);
                    Console.WriteLine("Description:  " + port.Caption);
                    Console.WriteLine("Manufacturer: " + port.Manufact);
                    Console.WriteLine("Device ID:    " + port.DeviceID);
                    Console.WriteLine("-----------------------------------");
                }
            }
            else if (args[0] == "print-devices" || args[0] == "pd")
            {
                Console.WriteLine("可用裝置種類如下：");
                Console.WriteLine($"    {"裝置名稱",-16}{"編號",-6}{"備註"}");
                foreach (var dev in device_list)
                {
                    Console.WriteLine($"  - {dev["name"],-20}{dev["dev_type"],-8}{dev["note"]}");
                }
            }
            else if (args[0] == "prog")
            {

            }
            else
                print_help();
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
                "  -h, --help            show this help message and exit\r\n" +
                "--lc LC               設置語言代碼。例如\"zh_TW\",\"en_US\"");
        }
        static void programming(string hex_file,string port,int baud=115200,int timeout=1,Action action=null)
        {

        }
    }
}
