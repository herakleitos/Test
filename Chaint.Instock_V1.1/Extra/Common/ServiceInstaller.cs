using System;
using System.Collections;

using System.Configuration.Install;
using System.ServiceProcess;
using System.Runtime.InteropServices;  
 

namespace CTWH.Common
{
    public class ServiceInstaller
    {
        public ServiceInstaller()
        { 
        }

          
        #region DLLImport     
  
        [DllImport("advapi32.dll")]     
        public static extern IntPtr OpenSCManager(string lpMachineName,string lpSCDB, int scParameter);     
        [DllImport("Advapi32.dll")]      
        public static extern IntPtr CreateService(IntPtr SC_HANDLE,string lpSvcName,string lpDisplayName,        
          int dwDesiredAccess,int dwServiceType, int dwStartType,int dwErrorControl,string lpPathName,       
          string lpLoadOrderGroup,int lpdwTagId, string lpDependencies,string lpServiceStartName,string lpPassword);    
        [DllImport("advapi32.dll")]       
        public static extern void CloseServiceHandle(IntPtr SCHANDLE);     
        [DllImport("advapi32.dll")]      
        public static extern int StartService(IntPtr SVHANDLE,int dwNumServiceArgs,string lpServiceArgVectors);     
        [DllImport("advapi32.dll",SetLastError=true)]      
        public static extern IntPtr OpenService(IntPtr SCHANDLE,string lpSvcName,int dwNumServiceArgs);     
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
        public bool InstallService(string svcPath, string svcName, string svcDispName)      
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
            int SERVICE_START =0x0010;        
            int SERVICE_STOP =0x0020;       
            int SERVICE_PAUSE_CONTINUE =0x0040;       
            int SERVICE_INTERROGATE =0x0080;       
            int SERVICE_USER_DEFINED_CONTROL =0x0100;        
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
            try    {        
                IntPtr sc_handle = OpenSCManager(null,null,SC_MANAGER_CREATE_SERVICE);       
                if (sc_handle.ToInt32() != 0)        
                {          
                    IntPtr sv_handle = CreateService(sc_handle,svcName,svcDispName,SERVICE_ALL_ACCESS,SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START,SERVICE_ERROR_NORMAL,svcPath,null,0,null,null,null);       
                    if(sv_handle.ToInt32() ==0)         
                    {         
                        CloseServiceHandle(sc_handle);         
                        return false;         
                    }        
                    else        
                    {         
                        //试尝启动服务        
                        int i = StartService(sv_handle,0,null);          
                        if(i==0)          
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
            catch(Exception e)    
      
            {        
                throw e;       
            }     
        }   
 ///        
        /// 反安装服务.     
        /// ///        
        /// 服务名.       
        public bool UnInstallService(string svcName)      
        {       
            int GENERIC_WRITE = 0x40000000;       
            IntPtr sc_hndl = OpenSCManager(null,null,GENERIC_WRITE);        
            if(sc_hndl.ToInt32() !=0)       
            {      int DELETE = 0x10000;      
                IntPtr svc_hndl = OpenService(sc_hndl,svcName,DELETE);       
                if(svc_hndl.ToInt32() !=0)        
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





        ///    
        /// 安装服务   
        ///    
        /// 服务名   
        /// 安装服务路径   
        public void InstallServiceByFilePath(IDictionary stateSaver, string filepath, string serviceName)
        {
            try
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (!ServiceIsExisted(serviceName))
                {
                    //Install Service   
                    AssemblyInstaller myAssemblyInstaller = new AssemblyInstaller();
                    myAssemblyInstaller.UseNewContext = true;
                    myAssemblyInstaller.Path = filepath;
                    myAssemblyInstaller.Install(stateSaver);
                    myAssemblyInstaller.Commit(stateSaver);
                    myAssemblyInstaller.Dispose();
                    //--Start Service   
                    service.Start();
                }
                else
                {
                    if (service.Status != System.ServiceProcess.ServiceControllerStatus.Running && service.Status != System.ServiceProcess.ServiceControllerStatus.StartPending)
                    {
                        service.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("installServiceError\n" + ex.Message);
            }
        }
        ///    
        /// 卸载windows服务   
        ///    
        /// 服务路径   
        public void UnInstallServiceByFilePath(string filepath, string serviceName)
        {
            try
            {
                if (ServiceIsExisted(serviceName))
                {
                    //UnInstall Service   
                    AssemblyInstaller myAssemblyInstaller = new AssemblyInstaller(); 
                    myAssemblyInstaller.UseNewContext = true;
                    myAssemblyInstaller.Path = filepath;
                    myAssemblyInstaller.Uninstall(null);
                    myAssemblyInstaller.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("unInstallServiceError\n" + ex.Message);
            }
        }
        ///    
        /// 检查服务是否存在   
        ///    
        /// 服务名   
        ///    
        public bool ServiceIsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    return true;
                }
            }
            return false;
        }  

 
    }






}
