using System;
using System.Windows.Forms;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.Security;
using Chaint.Common.Core.AppConfig;
using System.Management;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Chaint.Instock.Main
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //if (!Validate())
            //{
            //    return;
            //}
            try
            {
                System.Timers.Timer tim = new System.Timers.Timer(10000);
                tim.Elapsed += new System.Timers.ElapsedEventHandler(MemoryManage.ClearMemory);
                tim.AutoReset = true;
                tim.Enabled = true;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
                Context ctx = new Context("", "", Const_AppConfigFilePath.Ini, Const_ConfigSection.MainSection);
                ctx.DevicesConfigFilePath = Const_DevicesConfigFilePath.Ini;
                AppConfig_INI appConfiger = new AppConfig_INI(ctx.AppConfigFilePath);
                ctx.CompanyCode = appConfiger.GetValue("Company", "Code", "1");
                MainForm mainForm = new MainForm(ctx);
                mainForm.TopLevel = true;
                mainForm.WindowState = FormWindowState.Maximized;
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                ChaintMessageBox.Show("发生错误,请查看日志!");
            }
        }
    }
    public static class MemoryManage
    {

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        public static void ClearMemory(object source, System.Timers.ElapsedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
    }
}
