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
            byte[] datas = { 0xAC, 0xAC, 0xAC, 0x00, 0x0A, 0x01, 0x00, 0x05, 0x00, 0x05, 0x30, 0x31, 0x32, 0x33, 0x34, 0x05 };
            foreach (byte data in datas)
            {
                Console.WriteLine("input data : {0:x}", data);
                decode.put(data);
                
            }
            string[] res = decode.get();
            Console.WriteLine("ouput : {0}x{1}\n\t[{2}]",res[0],res[1],res[2]);
        }
    }
}
