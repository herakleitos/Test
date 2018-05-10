using System;
using System.Collections.Generic;
using System.Text;
using System.Management;    

//�������,Ҳ��Ҫ�������System.Manangement
namespace Chaint.Common.Devices.Utils
{
    /// <summary> 
    /// Computer Information 
    /// </summary> 
    public class OSSysInfo
    {
        public string CpuID;
        public string MacAddress;
        public string DiskID;
        public string IpAddress;
        public string LoginUserName;
        public string ComputerName;
        public string SystemType;
        public string TotalPhysicalMemory; //��λ��M 
        private static OSSysInfo _instance;

        public static OSSysInfo Instance()
        {
            if (_instance == null)
                _instance = new OSSysInfo();
            return _instance;
        }
        protected OSSysInfo()
        {
            CpuID = GetCpuID();
            MacAddress = GetMacAddress();
            DiskID = GetDiskID();
            IpAddress = GetIPAddress();
            LoginUserName = GetUserName();
            SystemType = GetSystemType();
            TotalPhysicalMemory = GetTotalPhysicalMemory();
            ComputerName = GetComputerName();
        }

        /// <summary>
        /// ��ȡ����ϵͳ���к�
        /// </summary>
        /// <returns></returns>
        public string GetOSSerialNumber()
        {
            string result = "";
            ManagementClass mClass = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moCollection = mClass.GetInstances();
            foreach (ManagementObject mObject in moCollection)
            {
                result += mObject["SerialNumber"].ToString() + " ";
            }
            return result;
        }


        /// <summary>
        /// ��ȡCPU���к�
        /// </summary>
        /// <returns></returns>
        string GetCpuID()
        {
            try
            {
                //��ȡCPU���кŴ��� 
                string cpuInfo = "";//cpu���к� 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
        }


        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public string GetMainBoardId()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root/CIMV2",
                    "SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moCollection = searcher.Get();
            foreach (ManagementObject mObject in moCollection)
            {
                result += mObject["SerialNumber"].ToString() + " ";
            }
            return result;
        }

        /// <summary>
        /// ��ȡӲ�����к�
        /// </summary>
        /// <returns></returns>
        string GetDiskID()
        {
            try
            {
                //��ȡӲ��ID 
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
        }


        /// <summary>
        /// ��ȡ������Mac��ַ
        /// </summary>
        /// <returns></returns>
        string GetMacAddress()
        {
            try
            {
                //��ȡ����Ӳ����ַ 
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>
        /// ��ȡIP��ַ
        /// </summary>
        /// <returns></returns>
        string GetIPAddress()
        {
            try
            {
                //��ȡIP��ַ 
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>
        /// ��ȡ������������Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetDriverInfo()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root/CIMV2", "SELECT * FROM Win32_LogicalDisk");
            ManagementObjectCollection moCollection = searcher.Get();
            foreach (ManagementObject mObject in moCollection)
            {
                //mObject["DriveType"]����6�п���ֵ���ֱ�����������壺
                //1:No type   2:Floppy disk   3:Hard disk
                //4:Removable drive or network drive   5:CD-ROM   6:RAM disk
                //����ֻ�г��̶���������Ӳ�̷����������
                if (mObject["DriveType"].ToString() == "3")
                {
                    result += string.Format("Name={0},FileSystem={1},Size={2},FreeSpace={3} ", mObject["Name"].ToString(),
                        mObject["FileSystem"].ToString(), mObject["Size"].ToString(), mObject["FreeSpace"].ToString());
                }
            }
            return result;
        }


        /// <summary> 
        /// ����ϵͳ�ĵ�¼�û��� 
        /// </summary> 
        /// <returns></returns> 
        string GetUserName()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["UserName"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary> 
        /// PC���� 
        /// </summary> 
        /// <returns></returns> 
        string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary> 
        /// �����ڴ� 
        /// </summary> 
        /// <returns></returns> 
        string GetTotalPhysicalMemory()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["TotalPhysicalMemory"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary> 
        /// ��ȡ�������
        /// </summary> 
        /// <returns></returns> 
        string GetComputerName()
        {
            try
            {
                return System.Environment.GetEnvironmentVariable("ComputerName");
            }
            catch
            {
                return "unknow";
            }
        }





    }
}
