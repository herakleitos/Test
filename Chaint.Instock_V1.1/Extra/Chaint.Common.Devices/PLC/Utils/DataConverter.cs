using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.PLC.Utils
{
    public class DataConverter
    {
        #region 转换函数
        /// <summary>
        ///		<remark>将byte数组转化为int</remark> 	
        ///</summary>  
        ///     <param name="buffer">PLC数据缓存</param>
        ///     <param name="Begin">开始位置</param>
        ///     <param name="Begin">结束位置</param>
        ///     <retvalue>返回数据</retvalue>		  
        public static ulong ByteToLong(byte[] buffer, int Begin, int End)
        {
            try
            {
                ulong retValue = 0;
                for (int i = End; i >= Begin; i--)
                {
                    retValue += buffer[i] * Convert.ToUInt64(System.Math.Pow(256, End - i));
                }
                return retValue;
            }
            catch
            {
                return 0;
            }
        }

        public static ulong ByteToLong(byte[] buffer)
        {
            try
            {
                ulong retValue = 0;
                for (int i = buffer.Length - 1; i >= 0; i--)
                {
                    retValue += buffer[i] * Convert.ToUInt64(System.Math.Pow(256, buffer.Length - 1 - i));
                }
                return retValue;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///		<remark>将int转化为byte数组</remark> 	
        ///</summary>  
        ///     <param name="num">Int数据</param>
        ///     <param name="w">数据位数</param>
        ///     <retvalue>返回Byte数组</retvalue>		  
        public static byte[] LongToByte(ulong num, int w)
        {
            byte[] buffer = new byte[w];
            for (int i = w - 1; i > 0; i--)
            {
                buffer[w - 1 - i] = (byte)(num / Convert.ToUInt64(System.Math.Pow(256, i)));
                num = num - (ulong)(buffer[w - 1 - i] * (int)Convert.ToUInt64(System.Math.Pow(256, i)));
            }
            buffer[w - 1] = (byte)(num);
            return buffer;
        }

        /// <summary>
        ///	<remark>将byte数组转化为int</remark> 	
        ///</summary>  
        ///     <param name="buffer">PLC数据缓存</param>
        ///     <param name="Begin">开始位置</param>
        ///     <param name="Begin">结束位置</param>
        ///     <retvalue>返回数据</retvalue>		  
        public static int ByteToInt(byte[] buffer, int Begin, int End)
        {
            try
            {
                int retValue = 0;
                for (int i = End; i >= Begin; i--)
                {
                    retValue += buffer[i] * Convert.ToInt32(System.Math.Pow(256, End - i));
                }
                return retValue;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///		<remark>将int转化为byte数组</remark> 	
        ///</summary>  
        ///     <param name="num">Int数据</param>
        ///     <param name="w">数据位数</param>
        ///     <retvalue>返回Byte数组</retvalue>		  
        public static byte[] IntToByte(int num, int w)
        {
            byte[] buffer = new byte[w];
            for (int i = w - 1; i > 0; i--)
            {
                buffer[w - 1 - i] = (byte)(num / Convert.ToInt32(System.Math.Pow(256, i)));
                num = num - buffer[w - 1 - i] * (int)Convert.ToInt32(System.Math.Pow(256, i));
            }
            buffer[w - 1] = (byte)(num);
            return buffer;

        }



        /// <summary>
        /// 将整数转换为数组 倒序方式
        /// </summary>
        /// <param name="val">待转换的数字</param>
        /// <param name="count">转换为2个或者4个字节</param>
        /// <returns></returns>
        private byte[] IntToBytes_Desc(int val, int count)
        {
            byte[] byts = new byte[count];

            for (int i = 0; i < count; i++)
            {
                byts[count - 1 - i] = (byte)(val >> (8 * (count - 1 - i)));
            }
            return byts;
        }

        /// <summary>
        /// 升序 方式  PLC对应方式  LongToByte,IntToByte
        /// </summary>
        /// <param name="val">待转换的数字</param>
        /// <param name="count">转换为2个或者4个字节</param>
        /// <returns></returns>
        private byte[] IntToBytes_ASC(int val, int count)
        {
            byte[] byts = new byte[count];

            for (int i = 0; i < count; i++)
            {
                byts[i] = (byte)(val >> (8 * (count - 1 - i)));
            }
            return byts;
        }


        /// <summary>
        /// 由十进制字符串转换成二进制字符串,参数值的数字不能超过255
        /// </summary>
        public static string DecToBin(string l_decStr)//倒序
        {
            int l_decVal = 0;
            int val = 0;
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(l_decStr))
                return "00000000";
            l_decVal = Convert.ToInt32(l_decStr);
            if (l_decVal > 255)
            {
                return "00000000";
            }
            while (l_decVal >= 2)
            {
                val = l_decVal % 2;
                l_decVal = l_decVal / 2;
                sb.Append(val);
            }
            sb.Append(l_decVal);
            return sb.ToString().PadRight(8, '0');
        }

        public static string DecToBin_Desc(string l_decStr)//正序
        {
            if (string.IsNullOrEmpty(l_decStr))
                return "00000000";
            try
            {
                byte byt = Convert.ToByte(l_decStr);
                string l_ret = Convert.ToString(byt, 2);
                l_ret = l_ret.PadLeft(8, '0');
                return l_ret;
            }
            catch
            {
                return "00000000";
            }
        }

        /// <summary>
        /// 将二进制形式的字符串转化成十进制数(二进制字符串保持为8位)
        /// </summary>
        public static int BinToDec(string l_BinStr)
        {
            char[] chr = l_BinStr.ToCharArray();
            int l_retVal = 0;

            if (chr.Length <= 0 || chr.Length > 8)
                return 0;
            for (int i = 0; i < chr.Length; i++)
            {
                if (chr[i] == '1')
                {
                    l_retVal += Convert.ToInt32(System.Math.Pow(2, chr.Length - 1 - i));
                }
            }
            return l_retVal;
        }

        /// <summary>
        /// 将倒序的8位(一个字节长)二进制数据字符串转换成正序的二进制数据
        /// </summary>
        /// <param name="l_BinStr"></param>
        /// <returns></returns>
        public static string ConvertBinToASC(string l_BinStr)
        {
            string l_retBinStr = "";
            if (l_BinStr.Length == 8)
            {
                for (int i = l_BinStr.Length - 1; i >= 0; i--)
                    l_retBinStr = l_retBinStr + l_BinStr.Substring(i, 1);
            }
            return l_retBinStr;
        }

        /// <summary>
        /// 判断某个数在转化为二进制时是否为1
        /// </summary>
        /// <param name="num">不大于256的数</param>
        /// <param name="pos">不大于8的数,某个位,从0开始</param>
        /// <returns></returns>
        public static bool IsOneInByte(int num, int pos)
        {
            return (num >> (pos) & 0x01) == 1;
        }

        /// <summary>
        /// 将某一位翻转(异或运算) 如:对10100001的第二位翻转 ^ 00000010
        /// </summary>
        /// <param name="num">数</param>
        /// <param name="pos">哪一位翻转,从0开始</param>
        public static int ConvertBit(int num, int pos)
        {
            return (num ^ Convert.ToInt32(System.Math.Pow(2, pos)));
        }

        /// <summary>
        /// 将某一位置值Flag 
        /// </summary>
        /// <param name="num">数</param>
        /// <param name="pos">某一位</param>
        /// <param name=flag>置1或0</param>
        public static int SetBitBoolValue(int num, int pos, bool flag)
        {
            if (flag)
            {
                num |= (0x1 << pos);
            }
            else
            {
                num &= ~(0x1 << pos);
            }
            return num;
        }


        /// <summary>
        /// 以字符串形式显示字节数组中的数字，以逗号分隔
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FormatByts(byte[] bytes, int start, int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = start; i < start + length; i++)
            {
                sb.AppendFormat("{0},",bytes[i]);
            }
            return sb.ToString().Trim(new char[] { ',' });
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

        public static byte[] StringToBytes(string str)
        {
            byte[] bs = new byte[str.Length];
            for (byte i = 0; i < 255; i++)
            {
                bs[i] = (byte)str[i];
            }
            return bs;
        }


        #endregion
    }
}
