using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Chaint.Common.Devices.Utils
{
    public class CEncodeConverter
    {
        public CEncodeConverter()
        {

        }

        /// <summary>
        /// 将某一对象序列化
        /// </summary>
        /// <param name="ObjectToSerialize"></param>
        /// <returns></returns>
        public static string Serialize(object ObjectToSerialize)
        {
            XmlSerializer Ser = new XmlSerializer(ObjectToSerialize.GetType());
            using (MemoryStream MS = new MemoryStream())
            {
                XmlTextWriter W = new XmlTextWriter(MS, new UTF8Encoding(false));
                W.Formatting = Formatting.Indented;
                Ser.Serialize(W, ObjectToSerialize);
                W.Flush();
                W.Close();
                return Utf8BytesToString(MS.ToArray());
            }
        }

        /// <summary>
        /// 将某一类型反序列化
        /// </summary>
        /// <param name="Xml"></param>
        /// <param name="ThisType"></param>
        /// <returns></returns>
        public static object Deserialize(string Xml, Type ThisType)
        {
            XmlSerializer myDeserializer = new XmlSerializer(ThisType);
            using (StringReader myReader = new StringReader(Xml))
            {
                return myDeserializer.Deserialize(myReader);
            }
        }

        public static string Utf8BytesToString(byte[] InBytes)
        {
            UTF8Encoding utf8encoder = new UTF8Encoding(false, true);
            return utf8encoder.GetString(InBytes);
        }

        public static byte[] StringToUtf8Bytes(string InString)
        {
            UTF8Encoding utf8encoder = new UTF8Encoding(false, true);
            return utf8encoder.GetBytes(InString);
        }

        /// <summary>
        /// Encode a url that is not already encoded
        /// </summary>
        /// <param name="url">The URL to encode.</param>
        /// <returns></returns>
        public static string UrlEncodeUrl(string url)
        {
            Uri uri = new Uri(url);
            if (!uri.UserEscaped && uri.Query != null && uri.Query.Length > 1)
            {
                StringBuilder sb = new StringBuilder(url.Length);
                sb.Append(uri.Scheme).Append(":").Append("//");
                sb.Append(uri.Host);
                if (!uri.IsDefaultPort)
                {
                    sb.Append(":").Append(uri.Port);
                }
                sb.Append(uri.AbsolutePath).Append("?");

                Encoding utf = Encoding.UTF8;

                string[] nameValue = uri.Query.Substring(1).Split('&');

                if (nameValue != null && nameValue.Length > 1)
                {
                    for (int i = 0; i < nameValue.Length; i++)
                    {
                        string[] parts = nameValue[i].Split('=');
                        sb.Append(parts[0]);
                        sb.Append("=");
                        sb.Append(System.Web.HttpUtility.UrlEncode(parts[1], utf));
                        if (i + 1 < nameValue.Length)
                        {
                            sb.Append("&amp;");
                        }
                    }

                    url = sb.ToString();
                }
            }

            return url;
        }

        /// <summary>
        /// 中文字符串转换为UTF_8码
        /// </summary>
        /// <param name="strGB2312">中文</param>
        /// <param name="retMsg">返回转换是否成功</param>
        /// <returns></returns>
        public static string GB2312ToUTF8(string strGB2312, ref string retMsg)
        {
            StringBuilder sb = new StringBuilder();
            UTF8Encoding utf8 = new UTF8Encoding();
            retMsg = "success";
            if (strGB2312.Trim().Length == 0)
            {
                retMsg = "fail";
                return "";
            }
            try
            {
                Byte[] encodedBytes = utf8.GetBytes(strGB2312);
                foreach (byte x in encodedBytes)
                {
                    sb.Append("%" + x.ToString("X"));
                }
                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// 中文字符串转换为UTF_8码
        /// </summary>
        /// <param name="strGB2312">中文</param>
        /// <returns></returns>
        public static string GB2312ToUTF8(string strGB2312)
        {
            StringBuilder sb = new StringBuilder();
            UTF8Encoding utf8 = new UTF8Encoding();
            if (strGB2312.Trim().Length == 0) return "";
            try
            {
                Byte[] encodedBytes = utf8.GetBytes(strGB2312);
                foreach (byte x in encodedBytes)
                {
                    sb.Append("%" + x.ToString("X"));
                }
                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return "";
            }
        }

        public static string GB2312ToUTF8_New(string strGB2312)
        {
            StringBuilder sb = new StringBuilder();
            UTF8Encoding utf8 = new UTF8Encoding();
            if (strGB2312.Trim().Length == 0) return "";
            try
            {
                Byte[] encodedBytes = utf8.GetBytes(strGB2312);
                foreach (byte x in encodedBytes)
                {
                    sb.Append(x.ToString("X"));
                }
                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return "";
            }
        }

        /// <summary>
        /// UTF_8码字符串(以%分隔)转换为中文字符串
        /// </summary>
        /// <param name="UTF8"></param>
        /// <param name="retMsg"></param>
        /// <returns></returns>
        public static string UTF8ToGB2312(string UTF8, ref string retMsg)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            retMsg = "success";
            if (UTF8.Trim().Length == 0)
            {
                retMsg = "fail";
                return "";
            }
            try
            {
                //以%分割字符串，并去掉空字符
                string[] chars = UTF8.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] bytUTF8 = new byte[chars.Length];

                for (int i = 0; i < chars.Length; i++)
                {
                    bytUTF8[i] = Convert.ToByte(chars[i], 16);
                }
                return utf8.GetString(bytUTF8);
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// UTF_8码字符串(以%分隔)转换为中文字符串
        /// </summary>
        /// <param name="UTF8">UTF8编码</param>
        /// <returns></returns>
        public static string UTF8ToGB2312(string UTF8)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            if (UTF8.Trim().Length == 0) return "";
            try
            {
                //以%分割字符串，并去掉空字符
                string[] chars = UTF8.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] bytUTF8 = new byte[chars.Length];

                for (int i = 0; i < chars.Length; i++)
                {
                    bytUTF8[i] = Convert.ToByte(chars[i], 16);
                }
                return utf8.GetString(bytUTF8);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 将十六进制数字符串(以%分隔)以某种编码方式转换为字符串形式.
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode">任一种编码格式,如:UTF8Encodeing,UnicodeEncoding等</param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            for (int i = 0; i < chars.Length; i++)  //逐个字符变为16进制字节数据
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }

            return encode.GetString(b);//按照指定编码将字节数组变为字符串
        }

        /// <summary>
        /// 将字符串以某种编码方式转换成十六进制字符串,并以%分隔各字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += "%" + Convert.ToString(b[i], 16);
            }
            return result;
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <param name="fenge">是否每字符用逗号分隔</param>
        /// <returns></returns>
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }

        ///<summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }


        ////十进制转二进制
        //Console.WriteLine("十进制166的二进制表示: "+Convert.ToString(166, 2));
        ////十进制转八进制
        //Console.WriteLine("十进制166的八进制表示: "+Convert.ToString(166, 8));
        ////十进制转十六进制
        //Console.WriteLine("十进制166的十六进制表示: "+Convert.ToString(166, 16));

        ////二进制转十进制
        //Console.WriteLine("二进制 111101 的十进制表示: "+Convert.ToInt32("111101", 2));
        ////八进制转十进制
        //Console.WriteLine("八进制 44 的十进制表示: "+Convert.ToInt32("44", 8));
        ////十六进制转十进制
        //Console.WriteLine("十六进制 CC的十进制表示: "+Convert.ToInt32("CC", 16)); 


    }
}
