using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Chaint.Common.Devices.Log
{
    public class LocalLogger
    {
        private string m_fileDirName = "";

        public LocalLogger(string strFileDirName)
        {
            m_fileDirName = strFileDirName;

            string fileDir = "";
            if (strFileDirName.Length > 0)
                fileDir = string.Format("{0}\\Log\\{1}\\{2}\\{3}",
                                            Application.StartupPath, m_fileDirName, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));
            else
                fileDir = string.Format("{0}\\Log\\{1}\\{2}",
                                            Application.StartupPath, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));

            DirectoryInfo di = new DirectoryInfo(fileDir);      //构造函数创建目录
            if (!di.Exists) di.Create();
        }

        public bool WriteFile(System.Windows.Forms.ListBox lstCntrl)
        {
            StreamWriter fileWriter = null;
            bool retValue = true;
            try
            {
                string fileDir = string.Format("{0}\\Log\\{1}\\{2}",
                                           System.Windows.Forms.Application.StartupPath, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));
                DirectoryInfo di = new DirectoryInfo(fileDir);      //构造函数创建目录
                if (!di.Exists) di.Create();
                string currFilePath = GetCurrFileName(fileDir);
                if (currFilePath.Length == 0)
                    retValue = false;

                if (lstCntrl.Items.Count > 0)
                {
                    if (!File.Exists(currFilePath))
                        fileWriter = File.CreateText(currFilePath);
                    else
                        fileWriter = File.AppendText(currFilePath);
                    fileWriter.WriteLine();
                    fileWriter.WriteLine("*************************************开始写入时间: {0}**************************************", DateTime.Now);
                    foreach (object obj in lstCntrl.Items)
                    {
                        fileWriter.WriteLine(obj);
                    }
                    fileWriter.Close();
                    lstCntrl.Items.Clear();
                    GC.Collect();
                }
                retValue = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                retValue = false;
            }
            return retValue;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public bool WriteFile(string strContent)
        {
            StreamWriter fileWriter = null;
            bool retValue = true;
            try
            {
                string fileDir = string.Format("{0}\\Log\\{1}\\{2}\\{3}",
                                           System.Windows.Forms.Application.StartupPath, m_fileDirName, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));
                DirectoryInfo di = new DirectoryInfo(fileDir);      //构造函数创建目录
                if (!di.Exists) di.Create();
                string currFilePath = GetCurrFileName(fileDir);
                if (currFilePath.Length == 0) retValue = false;

                if (!File.Exists(currFilePath))
                    fileWriter = File.CreateText(currFilePath);
                else
                    fileWriter = File.AppendText(currFilePath);

                lock (this)
                {
                    fileWriter.WriteLine(strContent);
                }
                fileWriter.Close();
                fileWriter.Dispose();
                fileWriter = null;

                retValue = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                retValue = false;
            }
            return retValue;
        }

        private string GetCurrFileName(string strFileDir)
        {
            string strCurrFileName = "";
            try
            {
                if (strFileDir.Trim().Length == 0)
                    return "";
                string strCurrTime = DateTime.Now.ToString("HH:mm:ss");
                string oneShift = "07:59:59";
                string twoShift = "15:59:59";
                string threeShift = "23:59:59";
                if (strCurrTime.CompareTo(oneShift) <= 0)
                    strCurrFileName = string.Format("{0}_00-08.log", DateTime.Now.ToString("dd"));
                else if (strCurrTime.CompareTo(oneShift) > 0 && strCurrTime.CompareTo(twoShift) <= 0)
                    strCurrFileName = string.Format("{0}_08-16.log", DateTime.Now.ToString("dd"));
                else if (strCurrTime.CompareTo(twoShift) > 0 && strCurrTime.CompareTo(threeShift) <= 0)
                    strCurrFileName = string.Format("{0}_16-24.log", DateTime.Now.ToString("dd"));
                if (strCurrFileName.Length == 0) return "";
                return string.Format("{0}\\{1}", strFileDir, strCurrFileName);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }





}
