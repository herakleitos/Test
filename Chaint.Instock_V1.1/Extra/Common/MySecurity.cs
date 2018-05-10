using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;   
namespace CTWH.Common
{
    public class MySecurity
    {
        #region   构造函数
        public MySecurity()
        {
        }
        #endregion

        #region   (   0   )Rijndael算法
        private SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();
        private static string Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
        ///   <summary>   
        ///   获得密钥   
        ///   </summary>   
        ///   <returns>密钥</returns>   
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        ///   <summary>   
        ///   获得初始向量IV   
        ///   </summary>   
        ///   <returns>初试向量IV</returns>   
        private byte[] GetLegalIV()
        {
            string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        ///   <summary>   
        ///   加密方法   
        ///   </summary>   
        ///   <param   name="Source">待加密的串</param>   
        ///   <returns>经过加密的串</returns>   
        public string RijndaelEncrypt(string Source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        ///   <summary>   
        ///   解密方法   
        ///   </summary>   
        ///   <param   name="Source">待解密的串</param>   
        ///   <returns>经过解密的串</returns>   
        public string RijndaelDecrypt(string Source)
        {
            byte[] bytIn = Convert.FromBase64String(Source);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        #endregion

        #region   (   1   )Base64与UTF8混用
        public string BUEncrypt(string bb)
        {
            byte[] by = new byte[bb.Length];
            by = System.Text.Encoding.UTF8.GetBytes(bb);

            string r = Convert.ToBase64String(by);
            return r;
        }

        public string BUDecrypt(string aa)
        {
            byte[] by = Convert.FromBase64String(aa);

            string r = Encoding.UTF8.GetString(by);
            return r;
        }

        #endregion

        #region   (   2   )倒序加1与顺序减1(中文支持不好)
        public string AddEncrypt(string rs)   //倒序加1加密       
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0;
            i <= rs.Length - 1;
            i++)
            {
                by[i] = (byte)((byte)rs[i] + 1);
            }
            rs = "";
            for (int i = by.Length - 1;
            i >= 0;
            i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }
        public string AddDecrypt(string rs)   //顺序减1解码       
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0;
            i <= rs.Length - 1;
            i++)
            {
                by[i] = (byte)((byte)rs[i] - 1);
            }
            rs = "";
            for (int i = by.Length - 1;
            i >= 0;
            i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }

        #endregion

        #region   (   3   )固定密钥算法
        private Byte[] Iv64 ={ 11, 22, 33, 44, 55, 66, 77, 88 };
        private Byte[] byKey64 ={ 10, 20, 30, 40, 50, 60, 70, 80 };
        //字符串加密   
        public string SKeyEncrypt(string strText)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //字符串解密   
        public string SKeyDecrypt(string strText)
        {
            Byte[] inputByteArray = new byte[strText.Length];
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey64, Iv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region   (   4   )DES算法
        public static byte[] DESKey = new byte[] { 0x82, 0xBC, 0xA1, 0x6A, 0xF5, 0x87, 0x3B, 0xE6, 0x59, 0x6A, 0x32, 0x64, 0x7F, 0x3A, 0x2A, 0xBB, 0x2B, 0x68, 0xE2, 0x5F, 0x06, 0xFB, 0xB8, 0x2D, 0x67, 0xB3, 0x55, 0x19, 0x4E, 0xB8, 0xBF, 0xDD };
        ///   <summary>   
        ///   DES加密   
        ///   </summary>   
        ///   <param   name="strSource">待加密字串</param>   
        ///   <param   name="key">32位Key值</param>   
        ///   <returns>加密后的字符串</returns>   
        public string DESEncrypt(string strSource)
        {
            return DESEncrypt(strSource, DESKey);
        }
        private string DESEncrypt(string strSource, byte[] key)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.Key = key;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.Zeros;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] byt = Encoding.Unicode.GetBytes(strSource);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        ///   <summary>   
        ///   DES解密   
        ///   </summary>   
        ///   <param   name="strSource">待解密的字串</param>   
        ///   <param   name="key">32位Key值</param>   
        ///   <returns>解密后的字符串</returns>   
        public string DESDecrypt(string strSource)
        {
            return DESDecrypt(strSource, DESKey);
        }
        private string DESDecrypt(string strSource, byte[] key)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.Key = key;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.Zeros;
            ICryptoTransform ct = sa.CreateDecryptor();
            byte[] byt = Convert.FromBase64String(strSource);
            MemoryStream ms = new MemoryStream(byt);
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs, Encoding.Unicode);
            return sr.ReadToEnd();
        }
        #endregion
    }   
}
