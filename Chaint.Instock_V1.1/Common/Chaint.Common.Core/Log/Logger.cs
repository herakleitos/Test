using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
using Chaint.Common.Core.Enums;
namespace Chaint.Common.Core.Log
{
    public static class Logger
    {
        private static string LogDir =
           AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\";
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="level"></param>
        public static void Log(ErrorInfo info,Enums_ErrorLevel level)
        {
            string fileName = string.Format("SystemInfo({0}).txt",DateTime.Now.Date.ToString("yyyy-MM-dd"));
            string filepath
                = string.Format("{0}{1}", LogDir, fileName);
            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite))
            {
                 fs.Position = fs.Length;
                using (StreamWriter writer = new StreamWriter(fs,Encoding.UTF8))
                {
                    writer.Write(FormatMsg(info, level));
                    writer.Flush();
                    writer.Dispose();
                }
                fs.Dispose();
            }
        }
        /// <summary>
        /// 流水日志
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            string fileName = string.Format("Flow({0}).txt", DateTime.Now.Date.ToString("yyyy-MM-dd"));
            string filepath
                = string.Format("{0}{1}", LogDir, fileName);
            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite))
            {
                fs.Position = fs.Length;
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.Write(message);
                    writer.Write("\r\n");
                    writer.Flush();
                    writer.Dispose();
                }
                fs.Dispose();
            }
        }
        /// <summary>
        /// 流水日志
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string fileNamePre,string message)
        {
            string fileName = string.Format("{0}({1}).txt", fileNamePre, DateTime.Now.Date.ToString("yyyy-MM-dd"));
            string filepath
                = string.Format("{0}{1}", LogDir, fileName);
            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite))
            {
                fs.Position = fs.Length;
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.Write(message);
                    writer.Write("\r\n");
                    writer.Flush();
                    writer.Dispose();
                }
                fs.Dispose();
            }
        }
        public static void Log(Exception ex)
        {
            ErrorInfo info = new ErrorInfo();
            info.Message = ex.Message;
            info.Source = ex.Source;
            info.Stack = ex.StackTrace;
            Enums_ErrorLevel level = Enums_ErrorLevel.Error;
            Log(info, level);
        }
        public static void Log(Exception ex, Enums_ErrorLevel level=Enums_ErrorLevel.Error)
        {
            ErrorInfo info = new ErrorInfo();
            info.Message = ex.Message;
            info.Source = ex.Source;
            info.Stack = ex.StackTrace;
            Log(info, level);
        }
        private static string FormatMsg(ErrorInfo info, Enums_ErrorLevel level)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("**************************************************");
            sb.AppendLine(string.Format("时间：{0}",DateTime.Now));
            sb.AppendLine(string.Format("错误类型：{0}", GetErrorLevel(level)));
            sb.AppendLine(string.Format("错误信息：{0}", info.Message));
            sb.AppendLine(string.Format("错误来源：{0}", info.Source));
            sb.AppendLine(string.Format("详细信息：{0}", info.Stack));
            sb.AppendLine("**************************************************");
            return sb.ToString();
        }
        private static string GetErrorLevel(Enums_ErrorLevel level)
        {
            string result = "普通";
            switch (level)
            {
                case Enums_ErrorLevel.Normal:
                    result = "普通";
                    break;
                case Enums_ErrorLevel.Waring:
                    result = "警告";
                    break;
                case Enums_ErrorLevel.Error:
                    result = "错误";
                    break;
            }
            return result;
        }
    }
}
