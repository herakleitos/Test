using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;

namespace CTWH
{
    static class Program
    {
        public static string AppName = "";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process instance = GetRunningInstance();
            if (instance != null)
            {
                //初始化程序配置信息
                //ApplicationSettings.Initialize();
                //Application.Run(new MainForm());
                HandleRunningInstance(instance);
            }
            else
            {
           Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                // Application.Run(new Form1());

                //初始化 access
                //access = new DataAccess.SQLAccess();
                
                Login dbconfig = new Login();
                DialogResult result = dbconfig.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // userinfo = dbconfig.userinfo;

                    Application.Run(new MainLayout());
                }
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                    Process.Start(Application.ExecutablePath);
                }
                else
                {
                    Application.Exit();
                }
            }
        }




        /// <summary>
        /// 获取应用程序的实例,没有其它的例程，返回Null
        /// </summary>
        /// <returns></returns>
        public static Process GetRunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            AppName = current.ProcessName;
            Process[] processes = Process.GetProcessesByName(AppName);
            //遍历正在有相同名字运行的例程
            foreach (Process process in processes)
            {
                //忽略现有的例程
                if (process.Id != current.Id)
                {
                    //确保例程从EXE文件运行
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //返回另一个例程实例
                        return process;
                    }
                }
            }
            return null;
        }

        private const int WS_SHOWNORMAL = 1;
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleRunningInstance(Process instance)
        {
            //确保窗口没有被最小化或最大化
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL);
            //设置真实例程为foreground window
            SetForegroundWindow(instance.MainWindowHandle);
        }
    }
}
