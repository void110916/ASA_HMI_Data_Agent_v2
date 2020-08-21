using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace usart
{
    enum state : UInt16
    {
        HEADER = 0,
        pkg_len = 1,
        switch_type = 2,
        chksum = 3,

        ar_type = 10,
        ar_num = 11,
        ar_dat_len = 12,
        ar_dat = 13,

        mt_type = 20,
        mt_numy = 21,
        mt_numx = 22,
        mt_dat_len = 23,
        mt_dat = 24,

        st_fs_len = 30,
        st_fs = 31,
        st_dat_len = 32,
        st_dat = 33
    }
    enum HMI_type : UInt16
    {
        int8 = 0,
        int16 = 1,
        int32 = 2,
        int64 = 3,
        uint8 = 4,
        uint16 = 5,
        uint32 = 6,
        uint64 = 7,
        float32 = 8,
        float64 = 9
    }
    
    public class Decode
    {
        public bool extraDecode = false;
        state decodeState = state.HEADER;
        UInt16 count = 0;
        UInt16 paclen = 0;
        UInt16 type = 0;
        UInt16 chksum = 0;
        UInt16 pkg_type = 0;

        UInt16 ar_type = 0;
        UInt16 ar_num = 0;
        UInt16 ar_dlen = 0;
        byte[] ar_dat;

        UInt16 mt_type = 0;
        UInt16 mt_numy = 0;
        UInt16 mt_numx = 0;
        UInt16 mt_dlen = 0;
        byte[] mt_dat;
        
        UInt16 st_fs_len = 0;
        byte[] st_fs;
        UInt16 st_dlen = 0;
        byte[] st_dat;
        public bool putEnable = false;
        public void put(byte buff)
        {
            if (extraDecode)
            {
                switch (decodeState)
                {
                    case state.HEADER:
                        if (buff == (byte)0xAC)
                        {
                            count++;
                            if (count == 3)
                            {
                                decodeState = state.pkg_len;
                                count = 0;
                            }
                        }
                        break;
                    case state.pkg_len:
                        if (count == 0)
                        {
                            paclen = (UInt16)(buff << 8);
                            count++;
                        }
                        else
                        {
                            paclen += (UInt16)(buff & 0xff);
                            count = 0;
                            decodeState = state.switch_type;
                        }
                        break;
                    case state.switch_type:
                        chksum += buff;
                        pkg_type = buff;
                        if (buff == 1)
                        {
                            decodeState = state.ar_type;
                        }
                        else if (buff == 2)
                        {
                            decodeState = state.mt_type;
                        }
                        else if (buff == 3)
                            decodeState = state.st_fs_len;
                        break;
                    case state.ar_type:
                        chksum += buff;
                        ar_type = buff;
                        decodeState = state.ar_num;
                        break;
                    case state.ar_num:
                        chksum += buff;
                        ar_num = buff;
                        decodeState = state.ar_dat_len;
                        break;
                    case state.ar_dat_len:
                        chksum += buff;
                        if (count == 0)
                        {
                            ar_dlen = (UInt16)(buff << 8);
                            count++;
                        }
                        else
                        {
                            ar_dlen += buff;
                            count = 0;
                            ar_dat = new byte[ar_dlen];
                            decodeState = state.ar_dat;
                        }
                        break;
                    case state.ar_dat:
                        chksum += buff;
                        ar_dat[count] = buff;
                        count++;
                        if (count == ar_dlen)
                        {
                            count = 0;
                            decodeState = state.chksum;
                        }
                        break;

                    case state.mt_type:
                        chksum += buff;
                        mt_type = buff;
                        decodeState = state.mt_numy;
                        break;
                    case state.mt_numy:
                        chksum += buff;
                        mt_numy = buff;
                        decodeState = state.mt_numx;
                        break;
                    case state.mt_numx:
                        chksum += buff;
                        mt_numx = buff;
                        decodeState = state.mt_dat_len;
                        break;
                    case state.mt_dat_len:
                        chksum += buff;
                        if (count == 0)
                        {
                            mt_dlen = (UInt16)(buff << 8);
                            count = 1;
                        }
                        else
                        {
                            mt_dlen += buff;
                            count = 0;
                            mt_dat = new byte[mt_dlen];
                            decodeState = state.mt_dat;
                        }
                        break;
                    case state.mt_dat:
                        chksum += buff;

                        mt_dat[count] = buff;
                        count++;
                        if (count == mt_dlen)
                        {
                            count = 0;
                            decodeState = state.chksum;
                        }
                        break;

                    case state.st_fs_len:
                        chksum += buff;
                        st_fs_len = buff;
                        st_fs = new byte[st_fs_len];
                        decodeState = state.st_fs;
                        break;
                    case state.st_fs:
                        chksum += buff;
                        st_fs[count] = buff;
                        count++;
                        if (count == st_fs_len)
                        {
                            count = 0;
                            decodeState = state.st_dat_len;
                        }
                        break;
                    case state.st_dat_len:
                        chksum += buff;
                        if (count == 0)
                        {
                            st_dlen = (UInt16)(buff << 8);
                            count = 1;
                        }
                        else
                        {
                            count = 0;
                            st_dlen += buff;
                            st_dat = new byte[st_dlen];
                            decodeState = state.st_dat;
                        }
                        break;
                    case state.st_dat:
                        chksum += buff;
                        st_dat[count] = buff;
                        count++;
                        if (count == st_dlen)
                        {
                            count = 0;
                            decodeState = state.chksum;
                        }
                        break;

                    case state.chksum:
                        decodeState = state.HEADER;
                        extraDecode = false;
                        if ((chksum & 0xFF) == buff)
                        {
                            putEnable = true;
                        }
                        break;
                }

            }
            else
            {
                if (buff == (byte)0xAC)
                {
                    extraDecode = true;
                    count++;
                }

            }
        }
        public string[] get()
        {
            
            if (!putEnable)
            {
                clear();
                return null;
            }
            string[] data = new string[3];
            if (pkg_type == 1)
            {

                data[0] = ((HMI_type)ar_type).ToString();
                data[1] = ar_num.ToString();
                data[2] = dataTransfirm((HMI_type)ar_type,ar_dat);
                clear();
                return data;
            }
            else if (pkg_type == 2)
            {
                data[0] = ((HMI_type)mt_type).ToString();
                data[1] = mt_numy.ToString() + 'x' + mt_numx.ToString();
                data[2] = Encoding.ASCII.GetString(mt_dat);
                clear();
                return data;
            }
            else if (pkg_type == 3)
            {
                data[0] = Encoding.ASCII.GetString(st_fs);
                data[1] = null;
                data[2] = Encoding.ASCII.GetString(st_dat);
                clear();
                return data;
            }
            else
            {
                return null;
            }
        }
        private void clear()
        {
            extraDecode = false;
            decodeState = state.HEADER;
            count = 0;
            paclen = 0;
            type = 0;
            chksum = 0;
            pkg_type = 0;

            ar_type = 0;
            ar_num = 0;
            ar_dlen = 0;
            ar_dat = null;

            mt_type = 0;
            mt_numy = 0;
            mt_numx = 0;
            mt_dlen = 0;
            mt_dat = null;

            st_fs_len = 0;
            st_fs = null;
            st_dlen = 0;
            st_dat = null;

            putEnable = false;
        }
        private string dataTransfirm(HMI_type type,byte[] data)
        {
            string opt = null;
            switch (type)
            {
                case HMI_type.int8:
                    sbyte[] d8 = new sbyte[data.Length];
                    Buffer.BlockCopy(data, 0, d8, 0, data.Length);
                    opt = string.Join(", ", d8);
                    break;
                case HMI_type.int16:
                    Int16[] d16=new Int16[data.Length/2];
                    Buffer.BlockCopy(data, 0, d16, 0, data.Length);
                    opt = string.Join(", ", d16);
                    break;
                case HMI_type.int32:
                    Int16[] d32 = new Int16[data.Length / 4];
                    Buffer.BlockCopy(data, 0, d32, 0, data.Length);
                    opt = string.Join(", ", d32);
                    break;
                case HMI_type.int64:
                    Int16[] d64 = new Int16[data.Length / 8];
                    Buffer.BlockCopy(data, 0, d64, 0, data.Length);
                    opt = string.Join(", ", d64);
                    break;
                case HMI_type.uint8:
                    Int16[] du8 = new Int16[data.Length];
                    Buffer.BlockCopy(data, 0, du8, 0, data.Length);
                    opt = string.Join(", ", du8);
                    break;
                case HMI_type.uint16:
                    UInt16[] du16=new ushort[data.Length/2];
                    Buffer.BlockCopy(data, 0, du16, 0, data.Length);
                    opt = string.Join(", ", du16);
                    break;
                case HMI_type.uint32:
                    Int16[] du32 = new Int16[data.Length / 2];
                    Buffer.BlockCopy(data, 0, du32, 0, data.Length);
                    opt = string.Join(", ", du32);
                    break;
                case HMI_type.uint64:
                    UInt64[] du64=new UInt64[data.Length/8];
                    Buffer.BlockCopy(data, 0, du64, 0, data.Length);
                    opt = string.Join(", ", du64);
                    break;
            }
            return opt;
        }
    }
}
