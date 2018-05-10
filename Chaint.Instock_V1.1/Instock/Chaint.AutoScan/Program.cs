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
using Chaint.Instock.Business;

namespace Chaint.AutoScan
{
    static class Program
    {
        private static Context context;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                bool createNew;
                using (System.Threading.Mutex mutx = new Mutex(true, Application.ProductName, out createNew))
                {
                    if (createNew)
                    {
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
                        Context ctx = new Context("", "", Const_AppConfigFilePath.Ini, Const_ConfigSection.MainSection);
                        ctx.DevicesConfigFilePath = Const_DevicesConfigFilePath.Ini;
                        AppConfig_INI appConfiger = new AppConfig_INI(ctx.AppConfigFilePath);
                        ctx.CompanyCode = appConfiger.GetValue("Company", "Code", "1");
                        context = ctx;
                        StockInAutoScan autoScan = new StockInAutoScan(ctx);
                        autoScan.TopLevel = true;
                        autoScan.WindowState = FormWindowState.Maximized;
                        autoScan.view.Open += OpenForm;
                        Application.Run(autoScan);
                    }
                    else
                    {
                        ChaintMessageBox.Show("应用程序已在运行中!");
                        System.Threading.Thread.Sleep(1000);
                        System.Environment.Exit(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                ChaintMessageBox.Show("发生错误,请查看日志!");
            }
        }
        private static void OpenForm(string formName, object data)
        {
            if (formName == Const_Option.Const_DirectionConfig)
            {
                DirectionConfig dirConfig = new DirectionConfig(context);
                dirConfig.TopMost = true;
                dirConfig.Show();
            }
        }
    }
}
