using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

/*程序说明：
 * 此加解密算法包含的算法有DES，三重DES，RC2算法，AES算法和RSA算法，
 * 其中RSA算法加解密速度比较慢，并且程序不能使用
 * 2009.12.24由hychong 整理编码
*/

namespace Chaint.Common.Devices.Security
{

    #region 加解密算法接口
    public enum AlgorithmType { DES,TRI_DES,RC2,AES,MD5}

    /// <summary>
    /// 解密
    /// </summary>
    public interface IDecryption
    {
        string DecodeText(string data);

        void DecodeFile(String inFileName, String outFileName);
        
    }

    /// <summary>
    /// 加密
    /// </summary>
    public interface IEncryption
    {
        string EncodeText(string data);
   
        void EncodeFile(String inFileName, String outFileName);
    }

    /// <summary>
    /// 加解密
    /// </summary>
    public interface IEnAndDeCryption:IEncryption,IDecryption
    {

    }

    public class EnAndDecryptorProvider
    {
        /// <summary>
        /// 创建加解密器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnAndDeCryption CreateEnAndDecryptor(AlgorithmType type)
        {
            IEnAndDeCryption iprovider = null;

            switch(type)
            {
                case AlgorithmType.DES:
                    iprovider = new DESEnAndDecryptor();
                    break;

                case AlgorithmType.TRI_DES:
                    iprovider = new TriDESEnAndDecryptor();
                    break;

                case AlgorithmType.RC2:
                    iprovider = new RC2EnAndDecryptor();
                    break;

                case AlgorithmType.AES:
                    iprovider = new AESEnAndDecryptor();
                    break;
            }

            return iprovider;
        }

        /// <summary>
        /// 创建加密器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEncryption CreateEncryptor(AlgorithmType type)
        {
            IEncryption iprovider = null;

            switch (type)
            {
                case AlgorithmType.DES:
                    iprovider = new DESEnAndDecryptor();
                    break;

                case AlgorithmType.TRI_DES:
                    iprovider = new TriDESEnAndDecryptor();
                    break;

                case AlgorithmType.RC2:
                    iprovider = new RC2EnAndDecryptor();
                    break;

                case AlgorithmType.AES:
                    iprovider = new AESEnAndDecryptor();
                    break;

                case AlgorithmType.MD5:
                    iprovider = new MD5Encryptor();
                    break;
            }

            return iprovider;
        }
    }





    #endregion

    #region DES算法
    class DESEnAndDecryptor : IEnAndDeCryption
    {
        private DES mydes;
        public string Key;
        public string IV;

        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public DESEnAndDecryptor(string key)
        {
            mydes = new DESCryptoServiceProvider();
            Key = key;
            IV = "728#$$%^TyguyshdsufhsfwofnhKJHJKHIYhfiusf98*(^%$^&&(*&()$##@%%$RHGJJHHJ";
        }
        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public DESEnAndDecryptor(string key, string iv)
        {
            mydes = new DESCryptoServiceProvider();
            Key = key;
            IV = iv;
        }

        public DESEnAndDecryptor()
        {
            mydes = new DESCryptoServiceProvider();
            Key = "ChaintCH";
            IV = "ChaintCH";
        }

        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mydes.GenerateKey();
            byte[] bytTemp = mydes.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = IV;
            mydes.GenerateIV();
            byte[] bytTemp = mydes.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        public string EncodeText(string data)
        {
            try
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(data);
                MemoryStream ms = new MemoryStream();
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本加密时出现错误，错误提示： \n" + ex.Message);
                return null;
            }
        }

        public string DecodeText(string data)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(data);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本解密时出现错误，错误提示： \n" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 加密方法byte[] to byte[]
        /// </summary>
        /// <param name="data">待加密的byte数组</param>
        /// <returns>经过加密的byte数组</returns>
        public byte[] EncodeText(byte[] data)
        {
            try
            {
                byte[] bytIn = data;
                MemoryStream ms = new MemoryStream();
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return bytOut;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本加密时出现错误，错误提示： \n" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 解密方法byte[] to byte[]
        /// </summary>
        /// <param name="data">待解密的byte数组</param>
        /// <returns>经过解密的byte数组</returns>
        public byte[] DecodeText(byte[] data)
        {
            try
            {
                byte[] bytIn = data;
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本解密时出现错误，错误提示： \n" + ex.Message);
                return null;
            }
        }

        //文本文件加密
        public void EncodeFile(String inFileName, String outFileName)
        {
            try
            {
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();

                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = mydes.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();
                }
                else  //文件名相同的情况
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待加密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;
                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);
                    byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
                    long rdlen = 0;              //This is the total number of bytes written.
                    long totlen = fin.Length;    //This is the total length of the input file.
                    int len;                     //This is the number of bytes to be written at a time.

                    ICryptoTransform encrypto = mydes.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件加密成功,加密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("文件加密时出现错误,错误提示： \n" + ex.Message);
            }
        }

        //文本文件解密
        public void DecodeFile(String inFileName, String outFileName)
        {

            try
            {
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;
                    ICryptoTransform encrypto = mydes.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();
                }
                else
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待解密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;

                    //创建文件流处理输入输出文件.
                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
                    long rdlen = 0;              //This is the total number of bytes written.
                    long totlen = fin.Length;    //This is the total length of the input file.
                    int len;                     //This is the number of bytes to be written at a time.

                    ICryptoTransform encrypto = mydes.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件解密成功,解密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文件解密时出现错误,错误提示： \n" + ex.Message);
            }
        }
    }
    #endregion

    #region 3重DES算法

    class TriDESEnAndDecryptor : IEnAndDeCryption
    {
        private TripleDES mydes;
        public string Key;
        public string IV;
        public TriDESEnAndDecryptor(string key)
        {
            mydes = new TripleDESCryptoServiceProvider();
            Key = key;
            IV = "#$^%&&*Yisifhsfjsljfslhgosdshf26382837sdfjskhf97(*&(*";
        }

        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public TriDESEnAndDecryptor(string key, string iv)
        {
            mydes = new TripleDESCryptoServiceProvider();
            Key = key;
            IV = iv;
        }

        public TriDESEnAndDecryptor()
        {
            mydes = new TripleDESCryptoServiceProvider();
            Key = "ChaintCH";
            IV = "ChaintCH";
        }

        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mydes.GenerateKey();
            byte[] bytTemp = mydes.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = IV;
            mydes.GenerateIV();
            byte[] bytTemp = mydes.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="data">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public string EncodeText(string data)
        {
            try
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(data);
                MemoryStream ms = new MemoryStream();
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本加密时出现错误,错误提示： \n" + ex.Message, "加密文件", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                return null;
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="data">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public string DecodeText(string data)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(data);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本解密时出现错误,错误提示： \n" + ex.Message, "加密文件", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                return null;
            }
        }

        /// <summary>
        /// 加密方法byte[] to byte[]
        /// </summary>
        /// <param name="data">待加密的byte数组</param>
        /// <returns>经过加密的byte数组</returns>
        private byte[] EncodeText(byte[] data)
        {
            try
            {
                byte[] bytIn = data;
                MemoryStream ms = new MemoryStream();
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return bytOut;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本加密时出现错误,错误提示： \n" + ex.Message, "加密文件", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                return null;
            }
        }

        /// <summary>
        /// 解密方法byte[] to byte[]
        /// </summary>
        /// <param name="data">待解密的byte数组</param>
        /// <returns>经过解密的byte数组</returns>
        private byte[] DecodeText(byte[] data)
        {
            try
            {
                byte[] bytIn = data;
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本解密时出现错误,错误提示： \n" + ex.Message, "加密文件", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                return null;
            }
        }

        /// <summary>
        /// 加密方法File to File
        /// </summary>
        /// <param name="inFileName">待加密文件的路径</param>
        /// <param name="outFileName">待加密后文件的输出路径</param>
        public void EncodeFile(string inFileName, string outFileName)
        {
            try
            {
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();

                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = mydes.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();
                }
                else
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待加密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;
                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = mydes.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件加密成功,加密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件加密时出现错误,错误提示： \n" + ex.Message);
            }
        }
        /// <summary>
        /// 解密方法File to File
        /// </summary>
        /// <param name="inFileName">待解密文件的路径</param>
        /// <param name="outFileName">待解密后文件的输出路径</param>
        public void DecodeFile(string inFileName, string outFileName)
        {
            try
            {
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();

                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;
                    ICryptoTransform encrypto = mydes.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();
                }
                else
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待解密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;
                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = mydes.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件解密成功,解密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件解密时出现错误,错误提示： \n" + ex.Message);
            }
        }
    }
    #endregion

    #region RC2算法

    class RC2EnAndDecryptor : IEnAndDeCryption
    {

        private RC2 rc;
        public string Key;
        public string IV;
        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public RC2EnAndDecryptor(string key)
        {
            rc = new RC2CryptoServiceProvider();
            Key = key;
            IV = "#$^%&&*Yisifhsfjsljfslhgosdshf26382837sdfjskhf97(*&(*";
        }
        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public RC2EnAndDecryptor(string key, string iv)
        {
            rc = new RC2CryptoServiceProvider();
            Key = key;
            IV = iv;
        }

        public RC2EnAndDecryptor()
        {
            rc = new RC2CryptoServiceProvider();
            Key = "ChaintCH";
            IV = "ChaintCH";
        }
        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            rc.GenerateKey();
            byte[] bytTemp = rc.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = IV;
            rc.GenerateIV();
            byte[] bytTemp = rc.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="data">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public string EncodeText(string data)
        {
            try
            {
                rc.Key = GetLegalKey();
                rc.IV = GetLegalIV();
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(data);
                MemoryStream ms = new MemoryStream();
                ICryptoTransform encrypto = rc.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本加密出现错误,错误提示： \n" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="data">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public string DecodeText(string data)
        {
            try
            {
                rc.Key = GetLegalKey();
                rc.IV = GetLegalIV();
                byte[] bytIn = Convert.FromBase64String(data);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                ICryptoTransform encrypto = rc.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本解密的时候出现错误！错误提示： \n" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 加密方法byte[] to byte[]
        /// </summary>
        /// <param name="Source">待加密的byte数组</param>
        /// <returns>经过加密的byte数组</returns>
        private byte[] Encrypt(byte[] Source)
        {
            try
            {
                byte[] bytIn = Source;
                MemoryStream ms = new MemoryStream();
                rc.Key = GetLegalKey();
                rc.IV = GetLegalIV();
                ICryptoTransform encrypto = rc.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return bytOut;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本加密出现错误，错误提示： \n" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 解密方法byte[] to byte[]
        /// </summary>
        /// <param name="Source">待解密的byte数组</param>
        /// <returns>经过解密的byte数组</returns>
        private byte[] Decrypt(byte[] Source)
        {
            try
            {
                byte[] bytIn = Source;
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                rc.Key = GetLegalKey();
                rc.IV = GetLegalIV();
                ICryptoTransform encrypto = rc.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本解密出现错误，错误提示： \n" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 加密方法File to File
        /// </summary>
        /// <param name="inFileName">待加密文件的路径</param>
        /// <param name="outFileName">待加密后文件的输出路径</param>

        public void EncodeFile(string inFileName, string outFileName)
        {
            try
            {
                rc.Key = GetLegalKey();
                rc.IV = GetLegalIV();

                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = rc.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();
                }
                else
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待加密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;

                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = rc.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件加密成功,加密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件加密时出现错误,错误提示： \n" + ex.Message);
            }
        }
        /// <summary>
        /// 解密方法File to File
        /// </summary>
        /// <param name="inFileName">待解密文件的路径</param>
        /// <param name="outFileName">待解密后文件的输出路径</param>
        public void DecodeFile(string inFileName, string outFileName)
        {
            try
            {
                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    rc.Key = GetLegalKey();
                    rc.IV = GetLegalIV();
                    ICryptoTransform encrypto = rc.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();
                }
                else
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待解密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;

                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    rc.Key = GetLegalKey();
                    rc.IV = GetLegalIV();
                    ICryptoTransform encrypto = rc.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件解密成功,解密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件解密时出现错误,错误提示： \n" + ex.Message);
            }
        }

    }
    #endregion

    #region AES算法(256位密钥)
    class AESEnAndDecryptor : IEnAndDeCryption
    {
        private RijndaelManaged myRijndael;
        public string Key;
        public string IV;
        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public AESEnAndDecryptor(string key)
        {
            myRijndael = new RijndaelManaged();
            Key = key;
            IV = "67^%*(&(*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
        }
        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public AESEnAndDecryptor(string key, string iv)
        {
            myRijndael = new RijndaelManaged();
            Key = key;
            IV = iv;
        }

        public AESEnAndDecryptor()
        {
            myRijndael = new RijndaelManaged();
            Key = "ChaintCH";
            IV = "ChaintCH";
        }

        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            myRijndael.GenerateKey();
            byte[] bytTemp = myRijndael.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = IV;
            myRijndael.GenerateIV();
            byte[] bytTemp = myRijndael.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        public string EncodeText(string data)
        {
            try
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(data);
                MemoryStream ms = new MemoryStream();
                myRijndael.Key = GetLegalKey();
                myRijndael.IV = GetLegalIV();
                ICryptoTransform encrypto = myRijndael.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本加密时出现错误，错误提示： \n" + ex.Message);
                return null;
            }
        }

        public string DecodeText(string data)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(data);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                myRijndael.Key = GetLegalKey();
                myRijndael.IV = GetLegalIV();
                ICryptoTransform encrypto = myRijndael.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("文本解密时出现错误，错误提示： \n" + ex.Message);
                return null;
            }
        }

        private byte[] EncodeText(byte[] data)
        {
            try
            {
                byte[] bytIn = data;
                MemoryStream ms = new MemoryStream();
                myRijndael.Key = GetLegalKey();
                myRijndael.IV = GetLegalIV();
                ICryptoTransform encrypto = myRijndael.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return bytOut;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
                return null;
            }
        }

        private byte[] DecodeText(byte[] data)
        {
            try
            {
                byte[] bytIn = data;
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                myRijndael.Key = GetLegalKey();
                myRijndael.IV = GetLegalIV();
                ICryptoTransform encrypto = myRijndael.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
                return null;
            }
        }


        public void EncodeFile(string inFileName, string outFileName)
        {
            try
            {
                myRijndael.Key = GetLegalKey();
                myRijndael.IV = GetLegalIV();
                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = myRijndael.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();
                }
                else
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待加密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;
                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;

                    ICryptoTransform encrypto = myRijndael.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件加密成功,加密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件加密时出现错误,错误提示： \n" + ex.Message);
            }
        }

        public void DecodeFile(string inFileName, string outFileName)
        {
            try
            {
                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;
                    myRijndael.Key = GetLegalKey();
                    myRijndael.IV = GetLegalIV();
                    ICryptoTransform encrypto = myRijndael.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();
                }
                else
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待解密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;
                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.SetLength(0);

                    byte[] bin = new byte[100];
                    long rdlen = 0;
                    long totlen = fin.Length;
                    int len;
                    myRijndael.Key = GetLegalKey();
                    myRijndael.IV = GetLegalIV();
                    ICryptoTransform encrypto = myRijndael.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                    while (rdlen < totlen)
                    {
                        len = fin.Read(bin, 0, 100);
                        cs.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }
                    cs.Close();
                    fout.Close();
                    fin.Close();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件解密成功,解密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件解密时出现错误,错误提示： \n" + ex.Message);
            }
        }
    }
    #endregion

    #region MD5 算法
    class MD5Encryptor:IEncryption
    {
        public MD5Encryptor()
        { }

        /// <summary>
        /// 加密文本 32位
        /// 
        /// 使用UTF-8编码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string EncodeText(string data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0');
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inFileName">原文件路径</param>
        /// <param name="outFileName">加密后文件路径</param>
        public void EncodeFile(string inFileName, string outFileName)
        {
            try
            {
               
                if (!inFileName.ToLower().Equals(outFileName.ToLower()))
                {
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    
                   
                    //读取原文件流数据至字节数组中
                    byte[] bytSource=new byte[fin.Length];
                    fin.Read(bytSource, 0, bytSource.Length);
                    fin.Close();
                    fin.Dispose();

                    //对字节数组加密

                    string resultAfterMD5=MD5Buffer(bytSource,0,bytSource.Length);

                    //将加密后的数据写入至文件中
                    byte[] bytMD5=Encoding.UTF8.GetBytes(resultAfterMD5);
                    FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    
                    
                    fout.Write(bytSource,0,bytSource.Length);
                    fout.Write(bytMD5,0,bytMD5.Length);
                    fout.Close();
                    fout.Dispose();

                }
                else
                {
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show("原文件路径与待加密文件路径完全相同，确定要覆盖原文件吗？", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (Result == System.Windows.Forms.DialogResult.Cancel)
                        return;
                    
                    FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                    
                    //读取原文件流数据至字节数组中
                    byte[] bytSource=new byte[fin.Length];
                    fin.Read(bytSource, 0, bytSource.Length);
                    fin.Close();
                    fin.Dispose();

                    //对字节数组加密
                    string resultAfterMD5=MD5Buffer(bytSource,0,bytSource.Length);

                    //将加密后的数据写入至文件中
                    byte[] bytMD5=Encoding.UTF8.GetBytes(resultAfterMD5);

                    string tmpName = System.Windows.Forms.Application.StartupPath + inFileName.Substring(inFileName.LastIndexOf('\\'));
                    FileStream fout = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                    fout.Write(bytSource,0,bytSource.Length);
                    fout.Write(bytMD5,0,bytMD5.Length);
                    fout.Close();
                    fout.Dispose();

                    File.Delete(inFileName);
                    File.Move(tmpName, inFileName);
                }
                MessageBox.Show("文件加密成功,加密后的文件在:" + inFileName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件加密时出现错误,错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="MD5File">MD5签名文件字符数组</param>
        /// <param name="index">计算起始位置</param>
        /// <param name="count">计算终止位置</param>
        /// <returns>计算结果</returns>
        private string MD5Buffer(byte[] MD5File, int index, int count)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Enctrypor = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash_byte = md5Enctrypor.ComputeHash(MD5File, index, count);
            string result = System.BitConverter.ToString(hash_byte);

            result = result.Replace("-", "");
            return result;
        }   
    }

    #endregion



}
