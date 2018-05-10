using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Threading;
using System.Drawing;
using System.Reflection;
using System.Net;
using System.Windows.Forms;
using DataModel;
using CTSocket;

namespace CTWH.Common
{
    public partial class Utils
    {

        public static void SaveAppSettingsToServer()
        {
            DateTime edittime = Utils.DateTimeNow;
            if (Utils.MSSqlAccess.AppConfigDeleteByPosition(Utils.Position))
            {
                foreach (string section in MyAppConfig.GetSectionNames())
                {
                    foreach (string entry in MyAppConfig.GetEntryNames(section))
                    {
                        Utils.MSSqlAccess.AppConfigInsertByValue(Utils.Position, Utils.LocalIPAddress, edittime, section, entry, GetConfigValue(section, entry));
                    }
                }

                //foreach (System.Configuration.KeyValueConfigurationElement element in Utils.MyAppConfig.AppSettings.Settings)                
                //{
                //    Utils.MSSqlAccess.AppConfigInsertByValue(Utils.Position, Utils.LocalIPAddress, edittime, element.Key, element.Value);
                //}
            }
        }

        public static void LoadAppSettingsFromServer()
        {
            MainDS ds = Utils.MSSqlAccess.AppConfigQueryByPosition(Utils.Position);
            foreach (MainDS.AppConfigRow row in ds.AppConfig.Rows)
            {
                if (row.ElementKey != "")
                    Utils.SetAppSettings(row.ElementKey, row.ElementValue);
            }
        }



        #region 程序集属性访问器

        public static Assembly LoadDLL(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return null;
            }
            else
            {
                return Assembly.LoadFile(path);
            }

        }


        public static string AssemblyTitle(Assembly assembly)
        {
            // 获取此程序集上的所有 Title 属性
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            // 如果至少有一个 Title 属性
            if (attributes.Length > 0)
            {
                // 请选择第一个属性
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                // 如果该属性为非空字符串，则将其返回
                if (titleAttribute.Title != "")
                    return titleAttribute.Title;
            }
            // 如果没有 Title 属性，或者 Title 属性为一个空字符串，则返回 .exe 的名称
            return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);

        }

        public static string AssemblyVersion(Assembly assembly)
        {
            return assembly.GetName().Version.ToString();

        }

        public static string AssemblyDescription(Assembly assembly)
        {

            // 获取此程序集的所有 Description 属性
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            // 如果 Description 属性不存在，则返回一个空字符串
            if (attributes.Length == 0)
                return "";
            // 如果有 Description 属性，则返回该属性的值
            return ((AssemblyDescriptionAttribute)attributes[0]).Description;

        }

        public static string AssemblyProduct(Assembly assembly)
        {

            // 获取此程序集上的所有 Product 属性
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            // 如果 Product 属性不存在，则返回一个空字符串
            if (attributes.Length == 0)
                return "";
            // 如果有 Product 属性，则返回该属性的值
            return ((AssemblyProductAttribute)attributes[0]).Product;

        }

        public static string AssemblyCopyright(Assembly assembly)
        {

            // 获取此程序集上的所有 Copyright 属性
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            // 如果 Copyright 属性不存在，则返回一个空字符串
            if (attributes.Length == 0)
                return "";
            // 如果有 Copyright 属性，则返回该属性的值
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;

        }

        public static string AssemblyCompany(Assembly assembly)
        {

            // 获取此程序集上的所有 Company 属性
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            // 如果 Company 属性不存在，则返回一个空字符串
            if (attributes.Length == 0)
                return "";
            // 如果有 Company 属性，则返回该属性的值
            return ((AssemblyCompanyAttribute)attributes[0]).Company;

        }
        #endregion

        //MES 形成传送给MES的 Socket数据信息

        /// <summary>
        /// 传递扫描信息给MES
        /// </summary>
        /// <param name="rollid">纸卷编号</param>
        /// <param name="position">位置</param>
        /// <param name="scantime">扫描时间</param>
        /// <returns></returns>
        public static string ToMESScanInfo(string rollid, string position, DateTime scantime)
        {

            //NNNN|0000000043|04|RollID|01|2008-05-21 10:00:00|>

            //功能代码04
            string data = "04|" + rollid + "|" + position + "|" + scantime.ToString("yyyy-MM-dd HH:mm:ss") + "|";

            return data;

        }

        /// <summary>
        /// 在称重喷墨工位发送纸卷的相关测量信息给MES
        /// </summary>
        /// <param name="rollid">纸卷编号</param>
        /// <param name="position">位置</param>
        /// <param name="width">宽度</param>
        /// <param name="diameter">直径</param>
        /// <param name="weight">重量</param>
        /// <returns></returns>
        public static string ToMESMeasureInfo(string rollid, string position, int width, int diameter, int weight)
        {
            ///例： NNNN|0000000039|05|RollID|01|0787|0980|1200|>
            ///功能代码 05
            string data = "05|" + rollid + "|" + position + "|" + width.ToString() + "|" + diameter.ToString() + "|" + weight.ToString() + "|";

            return data;

        }

        /// <summary>
        /// 纸卷相关信息更改后，CTD系统发送新信息给MES系统
        /// </summary>
        /// <param name="rollid">纸卷编号</param>
        /// <param name="editid">编辑属性代码</param>
        /// <param name="editvalue">编辑值</param>
        /// <returns></returns>
        public static string ToMESEditInfo(string rollid, string editid, string editvalue)
        {

            //例： NNNN|0000000039|06|RollID|01|01|>
            //功能代码06  
            string data = "06|" + rollid + "|" + editid + "|" + editvalue + "|";

            return data;

        }

        /// <summary>
        /// 请求MES纸卷的相关信息
        /// </summary>
        /// <param name="rollid">纸卷编号</param>
        /// <returns>数据字符串</returns>
        public static string ToMESRequestData(string rollid)
        {
            //例： NNNN|0000000058|08|RollID|>
            //功能代码08  
            string data = "08|" + rollid + "|";

            return data;
        }

        /// <summary>
        /// 在发送给MES之前拼装发送字符串
        /// </summary>
        /// <param name="onlyid">标志戳</param>
        /// <param name="data">数据</param>
        /// <returns>将要发送的数据，不含包尾</returns>
        public static string ToMESJoinData(string onlyid, string data)
        {

            string str = "|" + onlyid + "|" + data;

            str = (str.Length + 5).ToString("D4") + str;  //str.Length+5 表示需要包含包结束符以及NNNN 4位  包结束符在底层通信中已经包含  故需要 +1

            return str;
        }

        /// <summary>
        /// 验证RollID
        /// </summary>
        /// <param name="rollid"></param>
        /// <returns>Is RollID OK</returns> 
        public static bool VerifyBarcode(string barcode)
        {
            //如果条码设置长度为0,则不判断条码长度
            //条码 10位 或者 13 都有可能
            //if (Utils.BarcodeLength > 0 && barcode.Length != Utils.BarcodeLength)
            //    return false;

            foreach (char c in barcode)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public static bool VerifyIsNumber(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="serialport">串口控件</param>
        /// <param name="PortName">端口号</param>
        /// <returns>打开是否成功</returns>
        public static bool OpenSerialPort(System.IO.Ports.SerialPort serialport, DataType.SerialPortParameter spPara)
        {
            serialport.PortName = spPara.PortName;
            serialport.BaudRate = spPara.BaudRate;
            serialport.Parity = spPara.Parity;
            serialport.DataBits = spPara.DataBits;
            serialport.StopBits = spPara.StopBits;
            serialport.DtrEnable = spPara.DtrEnable;

            try
            {

                serialport.Open();
                return true;
            }
            catch
            {
                //MessageBoxEx.Show("打开串口" + serialport.PortName + "错误");
                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="serialport">串口控件</param>
        /// <param name="PortName">端口号</param>
        /// <returns>关闭是否成功</returns>
        public static bool CloseSerialPort(System.IO.Ports.SerialPort serialport)
        {
            if (!serialport.IsOpen)
                return true;
            try
            {
                serialport.Close();
                return true;
            }
            catch
            {
                MessageBox.Show("关闭串口" + serialport.PortName + "失败！");
                return false;
            }

        }

        /// <summary>
        /// 往串口发送字符
        /// </summary>
        /// <param name="serialPort">串口控件</param>
        /// <param name="p">发送字符</param>
        public static bool SendCharToScan(System.IO.Ports.SerialPort serialPort, char p)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write(p.ToString());
                return true;
            }
            else
                return false;
        }
        public static bool SendCharToScan(System.IO.Ports.SerialPort serialPort, string p)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write(p);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 往串口发送字符串
        /// </summary>
        /// <param name="serialPort">串口控件</param>
        /// <param name="str">发送字符串</param>
        public static bool SendStrToScan(System.IO.Ports.SerialPort serialPort, string str)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write(str);
                return true;
            }
            else
                return false;
        }

        //写txt log文档
        public static void WriteTxtLog(string fullfilename, string message)
        {
            if (System.IO.File.Exists(fullfilename))
            {
                System.IO.FileInfo info = new System.IO.FileInfo(fullfilename);
                if (info.Length > 2048 * 1024)
                {
                    info.Delete();
                }
            }

            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(fullfilename, System.IO.FileMode.Append);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);

                try
                {
                    sw.WriteLine("Time:" + Utils.DateTimeNow.ToString());
                    sw.WriteLine(message);
                    sw.WriteLine();
                    sw.Flush();
                }
                catch
                {

                }
                finally
                {
                    sw.Close();
                    sw = null;
                    fs.Close();
                }

            }
            catch { }


        }

        public static string ReadTxt(string fullfilename)
        {
            string retstr = "";
            if (System.IO.File.Exists(fullfilename))
            {
                using (System.IO.StreamReader sr = System.IO.File.OpenText(fullfilename))//path是你的txt文件的路径
                {
                    retstr = sr.ReadToEnd();
                }
            }

            return retstr;
        }
        
       
    } 
}
