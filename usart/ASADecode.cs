﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


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
    enum HMI_type : byte
    {
        i8 = 0,
        i16 = 1,
        i32 = 2,
        i64 = 3,
        ui8 = 4,
        ui16 = 5,
        ui32 = 6,
        ui64 = 7,
        f32 = 8,
        f64 = 9
    }
    public enum PAC_type : UInt16
    {
        ar = 1,
        mt = 2,
        st = 3
    }
    public struct HMI_format
    {
        public PAC_type type;
        public Match main_format;
        public Match[] child_format;
        public HMI_format(PAC_type type, Match main_format, Match[] child_format)
        {
            this.type = type;
            this.main_format = main_format;
            this.child_format = child_format;
        }
    }

    public class ASADecode
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
        List<byte> st_dat;
        public bool putEnable = false;
        public byte put(byte buff)
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
                            st_dat = new List<byte>(st_dlen);
                            decodeState = state.st_dat;
                        }
                        break;
                    case state.st_dat:
                        chksum += buff;
                        st_dat.Add(buff);
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
                return 0;
            }
            else
            {
                if (buff == (byte)0xAC)
                {
                    clear();
                    extraDecode = true;
                    count++;
                    return 0;
                }
                return buff;
            }
        }
        public string get()
        {
            if (!putEnable)
            {
                return null;
            }
            string[] data = new string[3];
            string text = null;
            if (pkg_type == (UInt16)PAC_type.ar)
            {

                data[0] = ((HMI_type)ar_type).ToString();
                data[1] = ar_num.ToString();
                data[2] = dataTransfirm((HMI_type)ar_type, ar_dat);
                text = string.Format("{0}_{1} :\r\n    {{ {2} }}\r\n\r\n", data[0], data[1], data[2]);
            }
            else if (pkg_type == (UInt16)PAC_type.mt)
            {
                data[0] = ((HMI_type)mt_type).ToString();
                data[1] = mt_numy.ToString() + 'x' + mt_numx.ToString();

                for (int i = 0; i < mt_numy; i++)
                {
                    string mt = dataTransfirm((HMI_type)mt_type, mt_dat.Skip((mt_dat.Length / mt_numy) * i).Take(mt_dat.Length / mt_numy).ToArray());
                    data[2] += "    { " + mt + " }\r\n";
                }
                text = string.Format("{0}_{1} :\r\n{{\r\n{2}}}\r\n\r\n", data[0], data[1], data[2]);

            }
            else if (pkg_type == (UInt16)PAC_type.st)
            {
                data[0] = Encoding.ASCII.GetString(st_fs);
                data[1] = null;
                text = data[0].Replace(",", " , ") + " :\r\n{\r\n";
                foreach (var tx in data[0].Split(','))
                {

                    string[] info = tx.Split('_');
                    string st = "";
                    switch (info[0])
                    {
                        case "ui8":
                            var dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(byte)).ToArray();
                            st = dataTransfirm(HMI_type.ui8, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]));
                            break;
                        case "ui16":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(UInt16)).ToArray();
                            st = dataTransfirm(HMI_type.ui16, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]) * 2);
                            break;
                        case "ui32":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(UInt32)).ToArray();
                            st = dataTransfirm(HMI_type.ui32, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]) * 4);
                            break;
                        case "ui64":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(UInt64)).ToArray();
                            st = dataTransfirm(HMI_type.ui64, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]) * 8);
                            break;
                        case "i8":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(sbyte)).ToArray();
                            st = dataTransfirm(HMI_type.i8, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]));
                            break;
                        case "i16":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(Int16)).ToArray();
                            st = dataTransfirm(HMI_type.i16, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]) * 2);
                            break;
                        case "i32":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(Int32)).ToArray();
                            st = dataTransfirm(HMI_type.i32, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]) * 4);
                            break;
                        case "i64":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(Int64)).ToArray();
                            st = dataTransfirm(HMI_type.i64, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]) * 8);
                            break;
                        case "f32":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(float)).ToArray();
                            st = dataTransfirm(HMI_type.f32, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]));
                            break;
                        case "f64":
                            dat = st_dat.GetRange(0, int.Parse(info[1]) * sizeof(double)).ToArray();
                            st = dataTransfirm(HMI_type.f64, dat);
                            st_dat.RemoveRange(0, int.Parse(info[1]));
                            break;
                    }
                    text += "    " + tx + " :\r\n        { " + st + " }\r\n";
                }
                text += "}\r\n\r\n";
            }
            else
            {
                return null;
            }
            clear();
            return text;
        }
        private void clear()
        {
            extraDecode = false;
            decodeState = state.HEADER;
            count = 0;
            paclen = 0;

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
        private string dataTransfirm(HMI_type type, byte[] data)
        {
            string opt = null;
            switch (type)
            {
                case HMI_type.i8:
                    sbyte[] d8 = new sbyte[data.Length];
                    Buffer.BlockCopy(data, 0, d8, 0, data.Length);
                    opt = string.Join(", ", d8);
                    break;
                case HMI_type.i16:
                    Int16[] d16 = new Int16[data.Length / 2];
                    Buffer.BlockCopy(data, 0, d16, 0, data.Length);
                    opt = string.Join(", ", d16);
                    break;
                case HMI_type.i32:
                    Int16[] d32 = new Int16[data.Length / 4];
                    Buffer.BlockCopy(data, 0, d32, 0, data.Length);
                    opt = string.Join(", ", d32);
                    break;
                case HMI_type.i64:
                    Int16[] d64 = new Int16[data.Length / 8];
                    Buffer.BlockCopy(data, 0, d64, 0, data.Length);
                    opt = string.Join(", ", d64);
                    break;
                case HMI_type.ui8:
                    Int16[] du8 = new Int16[data.Length];
                    Buffer.BlockCopy(data, 0, du8, 0, data.Length);
                    opt = string.Join(", ", du8);
                    break;
                case HMI_type.ui16:
                    UInt16[] du16 = new ushort[data.Length / 2];
                    Buffer.BlockCopy(data, 0, du16, 0, data.Length);
                    opt = string.Join(", ", du16);
                    break;
                case HMI_type.ui32:
                    Int16[] du32 = new Int16[data.Length / 2];
                    Buffer.BlockCopy(data, 0, du32, 0, data.Length);
                    opt = string.Join(", ", du32);
                    break;
                case HMI_type.ui64:
                    UInt64[] du64 = new UInt64[data.Length / 8];
                    Buffer.BlockCopy(data, 0, du64, 0, data.Length);
                    opt = string.Join(", ", du64);
                    break;
            }
            return opt;
        }
    }


    public class ASAEncode
    {
        private readonly byte[] _HEADER = { 0xac, 0xac, 0xac };

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
        List<byte> st_dat;

        byte[] data;
        // Regex st_g_re = new Regex(@"(i[0-9]{1,2}_[0-9]+(?:x[0-9]*)? *(?:, i[0-9]{1,2}_[0-9]+ )?):\s*({\s*.*\s*})+", RegexOptions.Compiled);  // structure global format regex
        //Regex mt_re = new Regex(@"(i[0-9]{1,2}_[0-9]+(?:x[0-9]*)? *(?:, i[0-9]{1,2}_[0-9]+ )?:\s*(((?<open>{)[^{}]*)+((?<-open>})\s*)+)(?(open)(?!)))", RegexOptions.Compiled);                       // array/matrix format regex
        Regex mt_re = new Regex(@"(?<head>(?:i[0-9]{1,2}_[0-9]+(?:x[0-9]*)? *(?:, i[0-9]{1,2}_[0-9]+ )?)):\s*(?:(?<open>{)(?<data>[^{}]*))+(?:(?<-open>})\s*)+(?(open)(?!))", RegexOptions.Compiled);                       // array/matrix format regex

        byte? getTypeNum(string typeStr)
        {
            switch (typeStr)
            {
                case "i8":
                    return 0;
                case "i16":
                    return 1;
                case "i32":
                    return 2;
                case "i64":
                    return 3;
                case "ui8":
                    return 4;
                case "ui16":
                    return 5;
                case "ui32":
                    return 6;
                case "ui64":
                    return 7;
                case "f32":
                    return 8;
                case "f64":
                    return 9;
                case "s":
                    return 15;
                default:
                    return null;
            }
        }

        byte[] encodeAr2Pac(sbyte[] data)
        {

            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.i8;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (byte d in data)
                payload.Add(d);
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }
        byte[] encodeAr2Pac(byte[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.ui8;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            payload.AddRange(data);
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }

        byte[] encodeAr2Pac(Int16[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.ui8;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (Int16 d in data)
                payload.AddRange(BitConverter.GetBytes(d));
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }

        byte[] encodeAr2Pac(UInt16[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.ui16;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (Int16 d in data)
                payload.AddRange(BitConverter.GetBytes(d));
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }
        byte[] encodeAr2Pac(Int32[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.i32;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (Int16 d in data)
                payload.AddRange(BitConverter.GetBytes(d));
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }
        byte[] encodeAr2Pac(UInt32[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.ui32;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (Int16 d in data)
                payload.AddRange(BitConverter.GetBytes(d));
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }
        byte[] encodeAr2Pac(Int64[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.i64;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (Int16 d in data)
                payload.AddRange(BitConverter.GetBytes(d));
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }
        byte[] encodeAr2Pac(UInt64[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.ui64;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (Int16 d in data)
                payload.AddRange(BitConverter.GetBytes(d));
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }
        byte[] encodeAr2Pac(float[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.f32;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (Int16 d in data)
                payload.AddRange(BitConverter.GetBytes(d));
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }
        byte[] encodeAr2Pac(double[] data)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;
            byte dtype = (byte)HMI_type.f64;
            UInt16 len = (UInt16)(data.Length & UInt16.MaxValue);
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add((byte)data.Length);
            payload.AddRange(BitConverter.GetBytes(len).Reverse());
            foreach (Int16 d in data)
                payload.AddRange(BitConverter.GetBytes(d));
            pac.AddRange(BitConverter.GetBytes((UInt16)payload.Count).Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));
            return pac.ToArray();
        }
        byte[] encodeAr2Pac(object[] data, byte dtype)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.ar;


            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            byte dlen = (byte)data.Length;
            payload.Add(dlen);

            List<byte> b_data = new List<byte>();
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, data);
                b_data.AddRange(ms.ToArray());
            }
            byte[] bdlen = BitConverter.GetBytes(b_data.Count);
            payload.AddRange(bdlen.Reverse());
            payload.AddRange(b_data);
            byte[] pac_len = BitConverter.GetBytes((UInt16)payload.Count);
            pac.AddRange(pac_len.Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));
            return pac.ToArray();
        }
        byte[] encodeMt2Pac(object[] data, byte dtype, byte dim1, byte dim2)
        {
            List<byte> pac = new List<byte>(_HEADER);
            byte pactype = (byte)PAC_type.mt;
            //byte dtype = HMI_type.
            List<byte> payload = new List<byte>();
            payload.Add(pactype);
            payload.Add(dtype);
            payload.Add(dim1);
            payload.Add(dim2);
            List<byte> b_data = new List<byte>();
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, data);
                b_data.AddRange(ms.ToArray());
            }
            byte[] b_dlen = BitConverter.GetBytes(b_data.Count);
            payload.AddRange(b_dlen.Reverse());
            payload.AddRange(b_data);
            byte[] pac_len = BitConverter.GetBytes(payload.Count);
            pac.AddRange(pac_len.Reverse());
            pac.AddRange(payload);
            pac.Add((byte)(payload.Select(x => (int)x).Sum() & 0xff));

            return pac.ToArray();
        }
        //byte[] encodeSt2Pac(object[] data,)
        //{

        //} 

        public HMI_format[] split(string text)
        {
            if (text == string.Empty)
                return new HMI_format[] { };
            List<HMI_format> ret = new List<HMI_format>();
            List<Match> matches = new List<Match>();
            var mt_mats = mt_re.Matches(text);
            string st_text = text;
            foreach (Match mat in mt_mats)
            {
                st_text.Replace(mat.Value, new string(' ', mat.Value.Length));
                matches.Add(mat);
            }
            var st_mat = mt_re.Matches(st_text);
            var st_child = new List<Match>();
            if (st_mat.Count > 0)
            {

                foreach (Match st in st_mat)
                {

                    var children = matches.FindAll(x => x.Index > st.Index && x.Index < (st.Index + st.Value.Length));
                    st_child.AddRange(children);
                    ret.Add(new HMI_format(PAC_type.st, st_mat[0], children.ToArray()));
                }
            }
            var mts = matches.Except(st_child);
            foreach (var mt in mts)
            {
                string header = mt.Groups[0].Value;
                if (header.Contains("x"))
                    ret.Add(new HMI_format(PAC_type.mt, mt, null));
                else
                    ret.Add(new HMI_format(PAC_type.ar, mt, null));
            }
            ret.Sort((x, y) => x.main_format.Index.CompareTo(y.main_format.Index));
            return ret.ToArray();

        }
        public bool put(HMI_format format)
        {
            Regex num_re = new Regex(@"([0-9]+.?[0-9]*)");
            string header = format.main_format.Groups[0].Value.Replace(":",string.Empty).Replace(" ",string.Empty);
            string text_pac = format.main_format.Groups[1].Value.Replace(" ",string.Empty);
            if (format.type == PAC_type.ar)
            {
                var type = header.Split('_')[0];
                var length = header.Split('_')[1];
                HMI_type hmi_type;
                if (type[0] == 'f')
                {
                    hmi_type = (HMI_type)(7+int.Parse(type.Substring(1))/32);

                }
                else if(type[0]=='i')
                {
                    hmi_type = (HMI_type)(int)Math.Log(int.Parse(type.Substring(1)), 2);
                }
                else if(type.StartsWith("ui"))
                {
                    hmi_type=(HMI_type)(int)Math.Log(int.Parse(type.Substring(2)), 2);
                }
                else 
                {
                    return false;
                }
                var len = int.Parse(length);
                var arr_s = text_pac.Replace("}", string.Empty).Split('{');
                //foreach
                //encodeAr2Pac(, (byte)hmi_type);
            }
            return true;
        }

        public bool put(string text)
        {
            if (text == string.Empty)
                return false;
            List<byte> pac = new List<byte>(_HEADER);
            var mt_mats = mt_re.Matches(text);
            string st_text = text;
            foreach (Match mat in mt_mats)
            {
                st_text.Replace(mat.Value, new string(' ', mat.Value.Length));
            }
            var st_mats = mt_re.Matches(st_text);
            if (st_mats.Count > 0)
                foreach (string format in text.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None))
                {

                    foreach (Match mat in mt_mats)
                    {
                        //if (mat.)
                }
                    //text.Remove()


            }
            return true;
        }
        public string get()
        {
            string text = "";
            return text;
        }
        public void clear()
        {
            data = null;
        }
    }

}
