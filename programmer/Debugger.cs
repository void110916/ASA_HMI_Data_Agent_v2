using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace programmer
{
    class Debugger
    {
        class IhexDebug
        {
            public static bool test_ihex_parse_1()
            {
                var predict = new Section[] { new Section(0, new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F }) };
                var ihex = new Ihex("./test_ihex/ihex_01.hex");
                var real = ihex.parse();
                return predict.SequenceEqual(real);
            }
            public static bool test_ihex_parse_2()
            {
                var predict = new Section[]{new Section(0,new byte[] {0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F }),
                                         new Section(0x100,new byte[]{0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F }) };
                var ihex = new Ihex("./test_ihex/ihex_02.hex");
                var real = ihex.parse();
                return predict.SequenceEqual(real.ToArray());
            }

            public static bool test_ihex_parse_3()
            {
                var predict = new Section[]{new Section(0,new byte[] {0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F }),
                                         new Section(0xabcd0100,new byte[]{0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F }) };
                var ihex = new Ihex("./test_ihex/ihex_03.hex");
                var real = ihex.parse();
                return predict.SequenceEqual(real.ToArray());
            }
            public static bool test_ihex_padding_1()
            {
                var input = new Section[]{new Section(0,new byte[] {0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F }),
                                         new Section(0xabcd0010,new byte[]{0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F }) };
                uint page_size = 256;
                byte space_data = 0xff;
                byte[] space_padding = Enumerable.Repeat((byte)0xff, 16).ToArray();

                var predict = new Section[2];
                var data1 = new byte[16 + space_padding.Length * 15];
                var org_data = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
                org_data.CopyTo(data1, 0);
                for (int i = 0; i < 15; i++)
                    space_padding.CopyTo(data1, 16 + i * space_padding.Length);
                predict[0] = new Section(0, data1);
                var data2 = new byte[16 + space_padding.Length * 15];
                space_padding.CopyTo(data2, 0);
                org_data.CopyTo(data2, space_padding.Length);
                for (int i = 0; i < 14; i++)
                    space_padding.CopyTo(data2, space_padding.Length * (1 + i) + 16);
                predict[1] = new Section(0xabcd0000, data2);

                var real = Ihex.padding_space(input.ToList(), page_size, space_data);
                return predict.SequenceEqual(real.ToArray());
            }
            public static bool test_ihex_padding_2()
            {
                var input = new Section[] { new Section(3, new byte[] { 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C }) };
                byte space_data = 0xff;
                uint page_sixe = 4;
                var predict = new Section[] { new Section(0, new byte[] { 0xFF, 0xFF, 0xFF, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0xFF, 0xFF, 0xFF }) };
                var real = Ihex.padding_space(input.ToList(), page_sixe, space_data);

                return predict.SequenceEqual(real.ToArray());

            }

            public static bool test_ihex_cut_1()
            {
                var input = new Section[]{new Section(0,new byte[] {0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F }),
                                         new Section(0xabcd0010,new byte[]{0x01,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F }) };
                int page_size = 4;
                var predict = new Section[]
                    {
                        new Section{
                            address=0,
                            data=new byte[]{0x00,0x01,0x02,0x03}
                        },
                        new Section{
                            address=4,
                            data=new byte[]{0x04,0x05,0x06,0x07}
                        },
                        new Section{
                            address=8,
                            data=new byte[]{0x08,0x09,0x0A,0x0B}
                        },
                        new Section{
                            address=12,
                            data=new byte[]{0x0C,0x0D,0x0E,0x0F}
                        },
                        new Section{
                            address=0xabcd0010,
                            data=new byte[]{0x01,0x01,0x02,0x03}
                        },
                        new Section{
                            address=0xabcd0014,
                            data=new byte[]{0x04,0x05,0x06,0x07}
                        },
                        new Section{
                            address=0xabcd0018,
                            data=new byte[]{0x08,0x09,0x0A,0x0B}
                        },
                        new Section{
                            address=0xabcd001c,
                            data=new byte[]{0x0C,0x0D,0x0E,0x0F}
                        },

                    };
                var real = Ihex.cut_to_pages(input.ToList(), page_size).ToArray();
                return predict.SequenceEqual(real);
            }
            //static int Main(string[] args)
            //{
            //    bool is_true = new int[] { 1, 2, 3 }.SequenceEqual(new int[] { 1, 3, 2 });
            //    is_true = test_ihex_parse_1();
            //    is_true &= test_ihex_parse_2();
            //    is_true &= test_ihex_parse_3();
            //    if (!is_true)
            //        Console.WriteLine("parse error");
            //    is_true = test_ihex_padding_1();
            //    is_true &= test_ihex_padding_2();
            //    if (!is_true)
            //        Console.WriteLine("padding error");
            //    is_true = test_ihex_cut_1();
            //    if (!is_true)
            //        Console.WriteLine("cut error");
            //    Console.WriteLine("finish ihex test");
            //    Console.ReadKey();
            //    return 0;

            //}

        }

        class CMDDebug : CMD
        {
            public CMDDebug(SerialPort serialPort) : base(serialPort) { }
            public CMDDebug() : base() { }
            public byte[] test_encode(CommanderHeader command, byte[] data) { return base.encode(command, data); }
            public bool test_decode()
            {
                var raw = new byte[] { 0xfc, 0xfc, 0xfc, 0xfa, 0x01, 0x00, 0x04, (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0xc0 };
                var predict = new COMMAND(CommanderHeader.CHK_PROTOCOL, new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' });
                var real = base.decode(base.encoder.GetString(raw));
                return predict.Equals(real);
            }

            public bool test_encode()
            {
                var predict = new byte[] { 0xfc, 0xfc, 0xfc, 0xfa, 0x01, 0x00, 0x04, (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0xc0 };
                var pac = new COMMAND(CommanderHeader.CHK_PROTOCOL, new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' });
                var real = base.encode(pac.command, pac.data);
                return predict.SequenceEqual(real);
            }

            //static int Main(string[] args)
            //{
            //    CMDDebug bug = new CMDDebug();
            //    var is_right = bug.test_decode();
            //    if (!is_right)
            //        Console.WriteLine("decode error");
            //    is_right = bug.test_encode();
            //    if (!is_right)
            //        Console.WriteLine("encode error");
            //    Console.WriteLine("finish CMD test");
            //    Console.ReadKey();
            //    return 0;

            //}
        }
    }
}
