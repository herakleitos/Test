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
    //��Ҫ������� System.ServiceProcess.dll ��System.Configuration.Install.Dll
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
        /// ��װ������      
        /// /// C#��װ����·��.      
        /// /// ������      
        /// /// ������ʾ����.      
        /// /// ����װ�Ƿ�ɹ�.      
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
                        //�Գ���������        
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
        ///  ��װ���������   �˴����������⣬�ڷ����б��п���������������������еķ����б����ܿ���
         /// </summary>
        /// <param name="svcPath">����·��. </param>
        /// <param name="svcName">������  </param>
        /// <param name="svcDispName">������ʾ����. </param>
        /// <param name="strDescription">������Ϣ </param>
        /// <returns>����װ�Ƿ�ɹ�.</returns>
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
                        //�Գ���������  
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
        /// ж�ط���
        /// </summary>
        /// <param name="svcName">����չ���Ƶķ�������</param>
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
        /// ��װ���� ����SC
        /// </summary>
        /// <param name="strServiceFilePath">��Ҫ��װ�ķ���·��</param>
        public static bool InstallService_Use_SC(string strServiceFilePath, string strServiceName, string strDescription)
        {
            try
            {
                System.ServiceProcess.ServiceController sc = GetCurrentServiceController(strServiceName);
                if (sc == null)    //��������ڴ˷���
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
                bool blnRet = StartServiceByName(strServiceName);       //��������
                return blnRet;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "��װ����");
                return false;
            }
        }

        /// <summary>
        /// �Ƴ���װĳ��Windows����
        /// </summary>
        /// <param name="strServiceFilePath"></param>
        public static bool UnInstallService_Use_SC(string strServiceFilePath, string strServiceName)
        {
            try
            {
                System.ServiceProcess.ServiceController sc = GetCurrentServiceController(strServiceName);
                if (sc != null)    //������ڴ˷���
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
                MessageBox.Show(ex.Message, "ж�ط���");
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
        /// �жϵ�ǰ�����Ƿ����
        /// </summary>
        /// <param name="strServiceName">����չ���ķ�������</param>
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
        /// ��ʼĳ�����
        /// </summary>
        /// <param name="strServiceName">����չ����</param>
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
                MessageBox.Show(ex.Message, "��������");
                return false;
            }
        }

        /// <summary>
        /// ֹͣĳ�����
        /// </summary>
        /// <param name="strServiceName">����չ����</param>
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
                MessageBox.Show(ex.Message, "ֹͣ����");
                return false;
            }
        }

        /// <summary>
        /// ��ͣĳ�����
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
                MessageBox.Show(ex.Message, "��ͣ����");
                return false;
            }
        }

        /// <summary>
        /// �ָ�����ĳ�����
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
                MessageBox.Show(ex.Message, "�ָ�����");
                return false;
            }
        }

        /// <summary>
        /// ��ȡĳһWindows�������������Ϣ
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
                    object retValue = service.GetValue("Description");    //��ȡ������Ϣ
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
                MessageBox.Show(ex.Message, "��ȡ��������");
                return "";
            }
        }

        /// <summary>
        /// ����ĳһWindows�������������Ϣ
        /// </summary>
        /// <param name="strServiceName">����չ����</param>
        /// <param name="strDescription">������Ϣ</param>
        /// <returns></returns>
        public static bool SetServiceDescription(string strServiceName, string strDescription)
        {
            try
            {
                // string strDescript = "����һ������TCPIPЭ���PLCͨѶWindows�������";
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
                MessageBox.Show(ex.Message, "����Windows��������");
                return false;
            }
        }


        /// <summary>
        /// ����SC���� ��װ����
        /// sc create binPath= {0} 
        /// type= own, share, interact, kernel, filesys  Ĭ�ϣ�own
        /// start= boot, system, auto, demand, disabled  Ĭ�ϣ�demand���ֶ���
        /// error= normal, severe, critical, ignore     //Ĭ����normal �������ڵ���ʧ�ܴ����������
        /// binPath=serviceFullPath             //����������ļ���·����������û��Ĭ��ֵ������ַ����Ǳ������õġ�
        /// group=(string)                      //����������ڵ��飬�������б�����ע����е�ServiceGroupOrder�¡�Ĭ����nothing
        /// depend=(space separated string)     //�пո���ַ���  �������������ǰ���������ķ�������ƻ�������
        /// Displayname=--(string)              //һ��Ϊ���û���������м����������ʹ�õ��ַ���
        /// password=--(string)
        /// Comments 
        /// 
        /// </summary>
        /// <param name="strServicePath">ȫ·��</param>
        /// <param name="strComments">��ʾ��Ϣ</param>
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
        /// ����SC���� ж�ط���
        /// </summary>
        /// <param name="strServicePath">ȫ·��</param>
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
        /// ����SC���� ��������
        /// </summary>
        /// <param name="strServicePath">ȫ·��</param>
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
        /// ֹͣ����
        /// </summary>
        /// <param name="strServicePath">ȫ·��</param>
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
        /// ִ��  Dos�������  
        /// </summary>
        /// <param name="dosCommand"></param>
        /// <returns></returns>
        private static string ExecuteCMD(string dosCommand)
        {
            return ExecuteCMD(dosCommand, 10);
        }

        /// <summary>  
        /// ִ��DOS�������DOS��������  
        /// </summary>  
        /// <param name="dosCommand">dos����</param>  
        /// <param name="milliseconds">�ȴ�����ִ�е�ʱ�䣨��λ�����룩��  
        /// ����趨Ϊ0�������޵ȴ�</param>  
        /// <returns>����DOS��������</returns>  
        private static string ExecuteCMD(string command, int seconds)
        {
            string output = ""; //����ַ���  
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//�������̶���  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//�趨��Ҫִ�е�����  
                startInfo.Arguments = "/C " + command;//��/C����ʾִ��������������˳�  
                startInfo.UseShellExecute = false;//��ʹ��ϵͳ��ǳ�������  
                startInfo.RedirectStandardInput = false;//���ض�������  
                startInfo.RedirectStandardOutput = true; //�ض������  
                startInfo.CreateNoWindow = true;//����������  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//��ʼ����  
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//�������޵ȴ����̽���  
                        }
                        else
                        {
                            process.WaitForExit(seconds); //�ȴ����̽������ȴ�ʱ��Ϊָ���ĺ���  
                        }
                        output = process.StandardOutput.ReadToEnd();//��ȡ���̵����  
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
