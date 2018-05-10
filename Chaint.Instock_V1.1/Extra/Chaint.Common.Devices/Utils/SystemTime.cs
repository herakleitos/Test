using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Chaint.Common.Devices.Utils
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

        public void FromDateTime(DateTime dateTime)
        {
            wYear = (ushort)dateTime.Year;
            wMonth = (ushort)dateTime.Month;
            wDayOfWeek = (ushort)dateTime.DayOfWeek;
            wDay = (ushort)dateTime.Day;
            wHour = (ushort)dateTime.Hour;
            wMinute = (ushort)dateTime.Minute;
            wSecond = (ushort)dateTime.Second;
            wMilliseconds = (ushort)dateTime.Millisecond;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond);
        }
    }

    /// <summary>
    /// 按哪种方式获取或设置
    /// </summary>
    public enum OperateType { API,CMD};

    public class SystemTime
    {
        private static SYSTEMTIME m_SysTime=new SYSTEMTIME();

        //设定，获取系统时间,SetSystemTime()默认设置的为UTC时间，比北京时间少了8个小时。
        [DllImport("Kernel32.dll")]
        private static extern bool SetSystemTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        private static extern bool SetLocalTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        private static extern void GetSystemTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        private static extern void GetLocalTime(ref SYSTEMTIME time);

        private static void SetLocalTimeByCMD(DateTime dt)
        {
            ProcessStartInfo PstartInfoCmd = new ProcessStartInfo();
            PstartInfoCmd.FileName = "cmd.exe";
            PstartInfoCmd.Arguments = string.Format("/C date {0} &time {1}", dt.ToShortDateString(), dt.ToShortTimeString());
            PstartInfoCmd.WindowStyle = ProcessWindowStyle.Hidden;

            Process proStep1 = new Process();
            proStep1.StartInfo = PstartInfoCmd;
            proStep1.Start();
            System.Threading.Thread.Sleep(100);
            proStep1.Close();
            proStep1.Dispose();
        }

        public static void SetLocalTime(DateTime dt)
        {
            SetLocalTime(OperateType.CMD, dt);
        }

        public static void SetLocalTime(OperateType opType,DateTime dt)
        {
            if (opType == OperateType.API)
            {
                m_SysTime.FromDateTime(dt);
                SetLocalTime(ref m_SysTime);
            }
            else
                SetLocalTimeByCMD(dt);
        }

        public static string GetLocalTime()
        {
            GetLocalTime(ref m_SysTime);
            return m_SysTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 系统时间要比本地时间少8个小时
        /// </summary>
        /// <returns></returns>
        public static string GetSystemTime()
        {
            GetSystemTime(ref m_SysTime);
            return m_SysTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        }



    }

  
       






}
