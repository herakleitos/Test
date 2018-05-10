using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace Chaint.Common.Devices.Utils
{
    //需要添加引用 System.ServiceProcess.dll 和System.Configuration.Install.Dll
    public class ServiceHelper
    {

        #region DLLImport

        [DllImport("advapi32.dll")]
        public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);
        [DllImport("Advapi32.dll")]
        public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
          int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
          string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);
        [DllImport("advapi32.dll")]
        public static extern void CloseServiceHandle(IntPtr SCHANDLE);
        [DllImport("advapi32.dll")]
        public static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);
        [DllImport("advapi32.dll")]
        public static extern int DeleteService(IntPtr SVHANDLE);
        [DllImport("kernel32.dll")]
        public static extern int GetLastError();

        #endregion DLLImport

        ///  
        /// 安装和运行      
        /// /// C#安装程序路径.      
        /// /// 服务名      
        /// /// 服务显示名称.      
        /// /// 服务安装是否成功.      
        public static bool InstallService_Use_API(string svcPath, string svcName, string svcDispName)
        {
            #region Constants declaration.
            int SC_MANAGER_CREATE_SERVICE = 0x0002;
            int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
            //int SERVICE_DEMAND_START = 0x00000003;        
            int SERVICE_ERROR_NORMAL = 0x00000001;
            int STANDARD_RIGHTS_REQUIRED = 0xF0000;
            int SERVICE_QUERY_CONFIG = 0x0001;
            int SERVICE_CHANGE_CONFIG = 0x0002;
            int SERVICE_QUERY_STATUS = 0x0004;
            int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
            int SERVICE_START = 0x0010;
            int SERVICE_STOP = 0x0020;
            int SERVICE_PAUSE_CONTINUE = 0x0040;
            int SERVICE_INTERROGATE = 0x0080;
            int SERVICE_USER_DEFINED_CONTROL = 0x0100;
            int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
                SERVICE_QUERY_CONFIG |
                SERVICE_CHANGE_CONFIG |
                SERVICE_QUERY_STATUS |
                SERVICE_ENUMERATE_DEPENDENTS |
                SERVICE_START |
                SERVICE_STOP |
                SERVICE_PAUSE_CONTINUE |
                SERVICE_INTERROGATE |
                SERVICE_USER_DEFINED_CONTROL);
            int SERVICE_AUTO_START = 0x00000002;
            #endregion Constants declaration.

            try
            {
                IntPtr sc_handle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
                if (sc_handle.ToInt32() != 0)
                {
                    IntPtr sv_handle = CreateService(sc_handle, svcName, svcDispName, SERVICE_ALL_ACCESS, SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START, SERVICE_ERROR_NORMAL, svcPath, null, 0, null, null, null);
                    if (sv_handle.ToInt32() == 0)
                    {
                        CloseServiceHandle(sc_handle);
                        return false;
                    }
                    else
                    {
                        //试尝启动服务        
                        int i = StartService(sv_handle, 0, null);
                        if (i == 0)
                        {
                            return false;
                        }
                        CloseServiceHandle(sc_handle);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

         /// <summary>
        ///  安装服务和运行   此处好像有问题，在服务列表中看不到，但在任务管理器中的服务列表中能看到
         /// </summary>
        /// <param name="svcPath">服务路径. </param>
        /// <param name="svcName">服务名  </param>
        /// <param name="svcDispName">服务显示名称. </param>
        /// <param name="strDescription">描述信息 </param>
        /// <returns>服务安装是否成功.</returns>
        public static bool InstallService_Use_API(string svcPath, string svcName, string svcDispName, string strDescription)
        {
            #region Constants declaration.
            int SC_MANAGER_CREATE_SERVICE = 0x0002;
            int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
            //int SERVICE_DEMAND_START = 0x00000003;        
            int SERVICE_ERROR_NORMAL = 0x00000001;
            int STANDARD_RIGHTS_REQUIRED = 0xF0000;
            int SERVICE_QUERY_CONFIG = 0x0001;
            int SERVICE_CHANGE_CONFIG = 0x0002;
            int SERVICE_QUERY_STATUS = 0x0004;
            int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
            int SERVICE_START = 0x0010;
            int SERVICE_STOP = 0x0020;
            int SERVICE_PAUSE_CONTINUE = 0x0040;
            int SERVICE_INTERROGATE = 0x0080;
            int SERVICE_USER_DEFINED_CONTROL = 0x0100;
            int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
                SERVICE_QUERY_CONFIG |
                SERVICE_CHANGE_CONFIG |
                SERVICE_QUERY_STATUS |
                SERVICE_ENUMERATE_DEPENDENTS |
                SERVICE_START |
                SERVICE_STOP |
                SERVICE_PAUSE_CONTINUE |
                SERVICE_INTERROGATE |
                SERVICE_USER_DEFINED_CONTROL);
            int SERVICE_AUTO_START = 0x00000002;
            #endregion Constants declaration.

            try
            {
                IntPtr sc_handle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
                if (sc_handle.ToInt32() != 0)
                {
                    IntPtr sv_handle = CreateService(sc_handle, svcName, svcDispName, SERVICE_ALL_ACCESS, SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START, SERVICE_ERROR_NORMAL, svcPath, null, 0, null, null, null);
                    if (sv_handle.ToInt32() == 0)
                    {
                        CloseServiceHandle(sc_handle);
                        return false;
                    }
                    else
                    {
                        //试尝启动服务  
                        SetServiceDescription(svcName, strDescription);
                        int i = StartService(sv_handle, 0, null);
                        if (i == 0)
                        {
                            return false;
                        }
                        CloseServiceHandle(sc_handle);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="svcName">带扩展名称的服务名称</param>
        /// <returns></returns>
        public static bool UnInstallService_Use_API(string svcName)
        {
            try
            {
                int GENERIC_WRITE = 0x40000000;
                IntPtr sc_hndl = OpenSCManager(null, null, GENERIC_WRITE);
                if (sc_hndl.ToInt32() != 0)
                {
                    int DELETE = 0x10000;
                    IntPtr svc_hndl = OpenService(sc_hndl, svcName, DELETE);
                    if (svc_hndl.ToInt32() != 0)
                    {
                        int i = DeleteService(svc_hndl);
                        if (i != 0)
                        {
                            CloseServiceHandle(sc_hndl);
                            return true;
                        }
                        else
                        {
                            CloseServiceHandle(sc_hndl);
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 安装服务 利用SC
        /// </summary>
        /// <param name="strServiceFilePath">需要安装的服务路径</param>
        public static bool InstallService_Use_SC(string strServiceFilePath, string strServiceName, string strDescription)
        {
            try
            {
                System.ServiceProcess.ServiceController sc = GetCurrentServiceController(strServiceName);
                if (sc == null)    //如果不存在此服务
                {
                    string[] cmdline = { };
                    // string serviceFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    TransactedInstaller transactedInstaller = new TransactedInstaller();
                    AssemblyInstaller assemblyInstaller = new AssemblyInstaller(strServiceFilePath, cmdline);
                    assemblyInstaller.UseNewContext = true;
                    transactedInstaller.Installers.Add(assemblyInstaller);
                    transactedInstaller.Install(new System.Collections.Hashtable());
                    transactedInstaller.Dispose();
                }
                SetServiceDescription(strServiceName, strDescription);
                bool blnRet = StartServiceByName(strServiceName);       //启动服务
                return blnRet;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "安装服务");
                return false;
            }
        }

        /// <summary>
        /// 移除安装某个Windows服务
        /// </summary>
        /// <param name="strServiceFilePath"></param>
        public static bool UnInstallService_Use_SC(string strServiceFilePath, string strServiceName)
        {
            try
            {
                System.ServiceProcess.ServiceController sc = GetCurrentServiceController(strServiceName);
                if (sc != null)    //如果存在此服务
                {
                    string[] cmdline = { };
                    // string serviceFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;

                    TransactedInstaller transactedInstaller = new TransactedInstaller();
                    AssemblyInstaller assemblyInstaller = new AssemblyInstaller(strServiceFilePath, cmdline);
                    assemblyInstaller.UseNewContext = true;
                    transactedInstaller.Installers.Add(assemblyInstaller);
                    transactedInstaller.Uninstall(null);
                    transactedInstaller.Dispose();
                }
                return true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "卸载服务");
                return false;
            }
        }

        public static string[] GetAllServicesDispName()
        {
            System.ServiceProcess.ServiceController[] serViceCntrls = System.ServiceProcess.ServiceController.GetServices();
            string[] tempServices = new string[serViceCntrls.Length];
            for (int i = 0; i < serViceCntrls.Length; i++)
            {
                tempServices[i] = serViceCntrls[i].DisplayName;
            }
            return tempServices;
        }

        public static string[] GetAllServicesName()
        {
            System.ServiceProcess.ServiceController[] serViceCntrls = System.ServiceProcess.ServiceController.GetServices();
            string[] tempServices = new string[serViceCntrls.Length];
            for (int i = 0; i < serViceCntrls.Length; i++)
            {
                tempServices[i] = serViceCntrls[i].ServiceName;
            }
            return tempServices;
        }

        /// <summary>
        /// 判断当前服务是否存在
        /// </summary>
        /// <param name="strServiceName">带扩展名的服务名称</param>
        /// <returns></returns>
        public static bool IsExistsService(string strServiceName)
        {
            System.ServiceProcess.ServiceController[] serViceCntrls = System.ServiceProcess.ServiceController.GetServices();
            string[] tempServices = new string[serViceCntrls.Length];
            for (int i = 0; i < serViceCntrls.Length; i++)
            {
                if (serViceCntrls[i].ServiceName == strServiceName)
                    return true;
            }
            return false;
        }

        public static System.ServiceProcess.ServiceController GetCurrentServiceController(string strServiceName)
        {
            if (strServiceName.Trim().Length == 0) return null;
            System.ServiceProcess.ServiceController[] serViceCntrls = System.ServiceProcess.ServiceController.GetServices();
            System.ServiceProcess.ServiceController sc = null;

            foreach (System.ServiceProcess.ServiceController currCntrl in serViceCntrls)
            {
                if (currCntrl.ServiceName.ToLower() == strServiceName.ToLower())
                {
                    sc = currCntrl;
                }
            }
            return sc;
        }

        /// <summary>
        /// 开始某项服务
        /// </summary>
        /// <param name="strServiceName">带扩展名称</param>
        public static bool StartServiceByName(string strServiceName)
        {
            try
            {
                if (strServiceName.Trim().Length == 0) return false;
                System.ServiceProcess.ServiceController sc = GetCurrentServiceController(strServiceName);
                if (sc != null)
                {
                    if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending)
                        sc.Start();
                    sc.Refresh();
                    return true;
                }
                else
                    return false;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "启动服务");
                return false;
            }
        }

        /// <summary>
        /// 停止某项服务
        /// </summary>
        /// <param name="strServiceName">带扩展名称</param>
        public static bool StopServiceByName(string strServiceName)
        {
            try
            {
                if (strServiceName.Trim().Length == 0) return false;
                System.ServiceProcess.ServiceController sc = GetCurrentServiceController(strServiceName);
                if (sc != null)
                {
                    if (sc.Status != ServiceControllerStatus.Stopped && sc.CanStop)
                        sc.Stop();
                    sc.Refresh();
                    return true;

                }
                else
                    return false;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "停止服务");
                return false;
            }
        }

        /// <summary>
        /// 暂停某项服务
        /// </summary>
        /// <param name="strServiceName"></param>
        public static bool PauseServiceByName(string strServiceName)
        {
            try
            {
                if (strServiceName.Trim().Length == 0) return false;
                System.ServiceProcess.ServiceController sc = GetCurrentServiceController(strServiceName);
                if (sc != null)
                {
                    if (sc.Status == ServiceControllerStatus.Running && sc.CanPauseAndContinue)
                        sc.Pause();
                    sc.Refresh();
                    return true;
                }
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "暂停服务");
                return false;
            }
        }

        /// <summary>
        /// 恢复继续某项服务
        /// </summary>
        /// <param name="strServiceName"></param>
        public static bool ContinueServiceByName(string strServiceName)
        {
            try
            {
                if (strServiceName.Trim().Length == 0) return false;
                System.ServiceProcess.ServiceController sc = GetCurrentServiceController(strServiceName);
                if (sc != null && sc.CanPauseAndContinue)
                {
                    if (sc.Status == ServiceControllerStatus.PausePending || sc.Status == ServiceControllerStatus.Paused)
                        sc.Continue();
                    sc.Refresh();
                    return true;
                }
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "恢复服务");
                return false;
            }
        }

        /// <summary>
        /// 获取某一Windows服务的描述性信息
        /// </summary>
        /// <param name="strServiceName"></param>
        /// <returns></returns>
        public static string GetServiceDescription(string strServiceName)
        {
            try
            {
                string retDescript = "";
                Microsoft.Win32.RegistryKey system, currentControlSet, services, service;
                //Open the HKEY_LOCAL_MACHINE\SYSTEM key
                system = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System");
                //Open CurrentControlSet
                currentControlSet = system.OpenSubKey("CurrentControlSet");
                //Go to the services key
                services = currentControlSet.OpenSubKey("Services");
                //Open the key for your service, and allow writing
                service = services.OpenSubKey(strServiceName, true);
                //Add your service's description as a REG_SZ value named "Description"

                if (service != null)
                {
                    //service.SetValue("Description", "This is my service's description.");
                    object retValue = service.GetValue("Description");    //获取描述信息
                    if (retValue == null)
                        retDescript = "";
                    else
                        retDescript = retValue.ToString();
                }
                else
                {
                    retDescript = "";
                }
                return retDescript;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "获取服务描述");
                return "";
            }
        }

        /// <summary>
        /// 设置某一Windows服务的描述性信息
        /// </summary>
        /// <param name="strServiceName">带扩展名称</param>
        /// <param name="strDescription">描述信息</param>
        /// <returns></returns>
        public static bool SetServiceDescription(string strServiceName, string strDescription)
        {
            try
            {
                // string strDescript = "这是一个基于TCPIP协议的PLC通讯Windows服务程序";
                Microsoft.Win32.RegistryKey system, currentControlSet, services, service;
                //Open the HKEY_LOCAL_MACHINE\SYSTEM key
                system = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System");
                //Open CurrentControlSet
                currentControlSet = system.OpenSubKey("CurrentControlSet");
                //Go to the services key
                services = currentControlSet.OpenSubKey("Services");
                //Open the key for your service, and allow writing
                service = services.OpenSubKey(strServiceName, true);
                //Add your service's description as a REG_SZ value named "Description"

                if (service != null)
                {
                    service.SetValue("Description", strDescription);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "设置Windows服务描述");
                return false;
            }
        }


        /// <summary>
        /// 利用SC命令 安装服务
        /// sc create binPath= {0} 
        /// type= own, share, interact, kernel, filesys  默认：own
        /// start= boot, system, auto, demand, disabled  默认：demand（手动）
        /// error= normal, severe, critical, ignore     //默认是normal 当服务在导入失败错误的严重性
        /// binPath=serviceFullPath             //服务二进制文件的路径名，这里没有默认值，这个字符串是必须设置的。
        /// group=(string)                      //这个服务属于的组，这个组的列表保存在注册表中的ServiceGroupOrder下。默认是nothing
        /// depend=(space separated string)     //有空格的字符串  在这个服务启动前必须启动的服务的名称或者是组
        /// Displayname=--(string)              //一个为在用户界面程序中鉴别各个服务使用的字符串
        /// password=--(string)
        /// Comments 
        /// 
        /// </summary>
        /// <param name="strServicePath">全路径</param>
        /// <param name="strComments">提示信息</param>
        /// <returns></returns>
        public static bool InstallServiceByCMD(string strServicePath)
        {

            if (!File.Exists(strServicePath)) return false;



            string strServiceName = Path.GetFileName(strServicePath);

            string strcmd = string.Format("sc create {0} binPath= {1} start= auto", strServiceName, strServicePath);
            string ret = ExecuteCMD(strcmd);

            strcmd = string.Format("sc config {0} type= interact type= own", strServiceName);
            ret = ExecuteCMD(strcmd);

            Console.Write(ret);

            return true;
        }

        /// <summary>
        /// 利用SC命令 卸载服务
        /// </summary>
        /// <param name="strServicePath">全路径</param>
        /// <returns></returns>
        public static bool UnInstallServiceByCMD(string strServicePath)
        {
            if (!File.Exists(strServicePath)) return false;

            string strServiceName = Path.GetFileName(strServicePath);
            string strcmd = string.Format("sc delete {0}", strServiceName);
            string ret = ExecuteCMD(strcmd);

            return true;
        }

        /// <summary>
        /// 利用SC命令 启动服务
        /// </summary>
        /// <param name="strServicePath">全路径</param>
        /// <returns></returns>
        public static bool StartServiceByCMD(string strServicePath)
        {
            if (!File.Exists(strServicePath)) return false;

            string strServiceName = Path.GetFileName(strServicePath);
            string strcmd = string.Format("sc start {0}", strServiceName);
            string ret = ExecuteCMD(strcmd);


            return true;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="strServicePath">全路径</param>
        /// <returns></returns>
        public static bool StopServiceByCMD(string strServicePath)
        {
            if (!File.Exists(strServicePath)) return false;

            string strServiceName = Path.GetFileName(strServicePath);
            string strcmd = string.Format("sc stop {0} 4:5:20", strServiceName);
            string ret = ExecuteCMD(strcmd);

            Console.Write(ret);
            return true;
        }

        /// <summary>
        /// 执行  Dos命令语句  
        /// </summary>
        /// <param name="dosCommand"></param>
        /// <returns></returns>
        private static string ExecuteCMD(string dosCommand)
        {
            return ExecuteCMD(dosCommand, 10);
        }

        /// <summary>  
        /// 执行DOS命令，返回DOS命令的输出  
        /// </summary>  
        /// <param name="dosCommand">dos命令</param>  
        /// <param name="milliseconds">等待命令执行的时间（单位：毫秒），  
        /// 如果设定为0，则无限等待</param>  
        /// <returns>返回DOS命令的输出</returns>  
        private static string ExecuteCMD(string command, int seconds)
        {
            string output = ""; //输出字符串  
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令  
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程  
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束  
                        }
                        else
                        {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }





    }
}
