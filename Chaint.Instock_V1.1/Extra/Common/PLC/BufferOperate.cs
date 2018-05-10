using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.PLC
{
    public class BufferOperate
    {
        /// <summary>
        /// SmallInt 转化为byte数组
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] SmallIntToByte(int i)
        {
            byte[] abyte0 = new byte[1];
            abyte0[0] = (byte)(0xff & i);
            return abyte0;
        }


        /// <summary>
        /// Int 转化为byte数组
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] IntToByte(int i)
        {
            byte[] abyte0 = new byte[2];
            abyte0[1] = (byte)(0xff & i);
            abyte0[0] = (byte)((0xff00 & i) >> 8);
            return abyte0;
        }

        /// <summary>
        /// DInt 转化为byte数组
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] DIntToByte(int i)
        {
            byte[] abyte0 = new byte[4];
            abyte0[3] = (byte)(0xff & i);
            abyte0[2] = (byte)((0xff00 & i) >> 8);
            abyte0[1] = (byte)((0xff0000 & i) >> 16);
            abyte0[0] = (byte)((0xff000000 & i) >> 24);
            return abyte0;
        }


        public static byte ConvertToByte(System.Collections.BitArray bits)
        {
            if (bits.Count > 8)
                throw new ArgumentException("ConvertToByte can only work with a BitArray containing a maximum of 8 values");
            byte result = 0;
            for (byte i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    result |= (byte)(1 << i);
            }
            return result;
        }



        /// <summary>
        /// byte数组转化为Int
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BytesToInt(byte[] bytes, int start)
        {
            int addr = bytes[start + 1] & 0xFF;
            addr |= ((bytes[start] << 8) & 0xFF00);
            return addr;
        }

        /// <summary>
        /// byte数组转化为DInt
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BytesToDInt(byte[] bytes, int start)
        {
            int addr = bytes[start + 3] & 0xFF;
            addr |= ((bytes[start + 2] << 8) & 0xFF00);
            addr |= ((bytes[start + 1] << 16) & 0xFF0000);
            addr |= (int)((bytes[start] << 24) & 0xFF000000);
            return addr;
        }

        /// <summary>
        /// byte数组转化为String
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] bytes, int start, int length)
        {
            string ret = "";
            for (int i = start; i < start + length; i++)
            {
                ret = ret + (char)bytes[i];
            }
            return ret;
        }

        /// <summary>
        /// byte数组转化为普通String RollID
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BytesToCString(byte[] bytes, int start, int length)
        {
            string ret = "";
            for (int i = start; i < start + length; i++)
            {
                char c = (char)bytes[i];
                if (Char.IsLetterOrDigit(c))
                    ret = ret + c;
                else
                {
                    break;
                }
            }
            return ret;
        }



        public static bool BitOfByte(Byte byte0, Int16 index)
        {
            return ((byte0 >> (index)) & 1) == 1 ? true : false;
            // return ((byte0 >> (7 - index)) & 1) == 1 ? true : false; 
        }

        public static bool BitOfBuffer(Byte[] Buffer, UInt16 Byteindex, UInt16 Bitindex)
        {
            return ((Buffer[Byteindex] >> (Bitindex)) & 1) == 1 ? true : false;   //测试结果 无需取反
            // return ((Buffer[Byteindex] >> (7 - Bitindex)) & 1) == 1 ? true : false; 
        }


        public static bool BitOfBuffer(Byte[] Buffer, double index)
        {
            string str = index.ToString("f1");
            return BitOfBuffer(Buffer, str);

        }

        /// <summary>
        /// 通过传入的字符串截取Buffer的bit  传入 如"5.3"
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="str">传入的字符串 例如"5.4"</param>
        /// <returns></returns>
        public static bool BitOfBuffer(Byte[] Buffer, string str)
        {
            string[] strs = str.Split('.');
            UInt16 Byteindex = Convert.ToUInt16(strs[0]);
            UInt16 Bitindex = Convert.ToUInt16(strs[1]);   //测试结果 高位
            return BitOfBuffer(Buffer, Byteindex, Bitindex);
        }


        #region 其他的Byte 互转 String 的函数

        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut += String.Format("{0:X2}", InByte);
            }
            return StringOut;
        }

        public static string ByteToString(byte[] InBytes, string Param) ///为了显示方便可能需要在每个字节后面补上一个空格等）
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2}", InByte) + Param;
            }
            StringOut = StringOut.Substring(0, StringOut.Length - Param.Length);
            return StringOut;
        }


        public static string ByteToString(byte iIn)
        {
            string sTmp = "";
            //   sTmp = Supply00( iIn.ToString("X") , 0, "0", 2);
            sTmp = sTmp + String.Format("{0:X2}", iIn);
            return sTmp;
        }
        public static string StrToHexAsc(string inString)
        {
            char[] inStringToChar = inString.ToCharArray();
            string outString = "";
            for (int i = 0; i < inStringToChar.Length; i++)
            {
                outString += String.Format("{0:X2}", Convert.ToByte(inStringToChar[i]));
            }
            return outString;
        }


        public static byte[] StringToByte(string InString, string Param)
        {
            string[] ByteStrings;
            ByteStrings = InString.Split(Param.ToCharArray());

            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++)
            {
                ByteOut[i] = Convert.ToByte(ByteStrings[i], 16);
            }

            return ByteOut;
        }

        public static byte[] StringToByte(string InString)
        {
            string[] ByteStrings;
            ByteStrings = new string[InString.Length / 2];
            for (int i = 0; i < ByteStrings.Length; i++)
            {
                ByteStrings[i] = InString.Substring(i * 2, 2);
            }

            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++)
            {
                ByteOut[i] = Convert.ToByte(ByteStrings[i], 16);
            }

            return ByteOut;
        }



        //补00函数
        public static string Supply00(string sOldStr, int SupplyWay, string SupplyChar, long SupplyNum)
        {
            int i;
            string sTmp;

            sTmp = "";
            for (i = sOldStr.Length; i < SupplyNum; i++)
                sTmp = sTmp + SupplyChar;
            switch (SupplyWay)
            {
                case 0: //前补充
                    sTmp = sTmp + sOldStr;
                    break;
                case 1: //后补充
                    sTmp = sOldStr + sTmp;
                    break;
                default:
                    sTmp = sOldStr;
                    break;
            }
            return sTmp;
        }


        public static string HexAscToStr(string inString)
        {

            byte[] CardData;
            CardData = StringToByte(inString);

            string outString = Encoding.ASCII.GetString(CardData);
            return outString;
        }

        #endregion


    }
}
