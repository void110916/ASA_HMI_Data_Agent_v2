using System;
using System.Text;
using usart;
namespace decode_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Decode decode = new Decode();
            byte[] datas = { 0x00 ,0xFF ,0x02 ,0xFD ,0x04 ,0x05 ,0x00 ,0x06 ,0x00 ,0x07 ,0x00 ,0x08 ,0x00 ,0x09 ,0x00 };
            Console.WriteLine(Encoding.ASCII.GetString(datas));
            //foreach (byte data in datas)
            //{
                
            //    decode.put(data);
            //    if (decode.putEnable)
            //    
            //        string[] res = decode.get();
            //        Console.WriteLine("ouput : {0}x{1}\n\t[{2}]", res[0], res[1], res[2]);
            //    }
            //}
            
        }
    }
}
