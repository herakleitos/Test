using System;
using Microsoft.Win32;

namespace Chaint.Common.Devices.Utils
{
    public class CRegistryHelper
    {
        public static bool WriteToRegistry(string KeyName, string KeyValue)
        {
            RegistryKey regHive = Microsoft.Win32.Registry.LocalMachine;
            string regPath = "SOFTWARE\\Chaint";
            return WriteToRegistry(regHive, regPath, KeyName, KeyValue);
        }

        /// <summary>
        /// 写注册表
        /// </summary>
        /// <param name="strAppName">应用程序名称(段名称)</param>
        /// <param name="strKeyName">键名称</param>
        /// <param name="strKeyValue">键值</param>
        /// <returns></returns>
        public static bool WriteToRegistry(string strAppName,string strKeyName,string strKeyValue)
        {
            RegistryKey regHive = Microsoft.Win32.Registry.LocalMachine;
            string regPath = string.Format("SOFTWARE\\{0}",strAppName);
            return WriteToRegistry(regHive, regPath, strKeyName, strKeyValue);
        }

        public static bool WriteToRegistry(RegistryKey RegHive, string RegPath, string KeyName, string KeyValue)
        {
            // Split the registry path 
            string[] regStrings;
            regStrings = RegPath.Split('\\');
            // First item of array will be the base key, so be carefull iterating below
            RegistryKey[] RegKey = new RegistryKey[regStrings.Length + 1];
            RegKey[0] = RegHive;

            for (int i = 0; i < regStrings.Length; i++)
            {
                RegKey[i + 1] = RegKey[i].OpenSubKey(regStrings[i], true);
                // If key does not exist, create it
                if (RegKey[i + 1] == null)
                {
                    RegKey[i + 1] = RegKey[i].CreateSubKey(regStrings[i]);
                }
            }
            // Write the value to the registry
            try
            {
                RegKey[regStrings.Length].SetValue(KeyName, KeyValue);
                return true;
            }
            catch (System.NullReferenceException ex1)
            {
                Console.WriteLine(ex1.Message);
                return false;
                
            }
            catch (System.UnauthorizedAccessException ex2)
            {
                Console.WriteLine(ex2.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static string ReadFromRegistry(string KeyName, string DefaultValue)
        {
            RegistryKey regHive = Microsoft.Win32.Registry.LocalMachine;
            string regPath = "SOFTWARE\\Chaint";
            return ReadFromRegistry(regHive, regPath, KeyName, DefaultValue);
        }

        /// <summary>
        /// 读注册表
        /// </summary>
        /// <param name="strAppName">应用程序名称(段名称)</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">键值</param>
        /// <returns></returns>
        public static string ReadFromRegistry(string strAppName,string KeyName, string DefaultValue)
        {
            RegistryKey regHive = Microsoft.Win32.Registry.LocalMachine;
            string regPath = string.Format("SOFTWARE\\{0}", strAppName);
            return ReadFromRegistry(regHive, regPath, KeyName, DefaultValue);
        }  

        public static string ReadFromRegistry(RegistryKey RegHive, string RegPath, string KeyName, string DefaultValue)
        {
            string[] regStrings;
            string result = "";

            regStrings = RegPath.Split('\\');
            //First item of array will be the base key, so be carefull iterating below
            RegistryKey[] RegKey = new RegistryKey[regStrings.Length + 1];
            RegKey[0] = RegHive;

            try
            {
                for (int i = 0; i < regStrings.Length; i++)
                {
                    RegKey[i + 1] = RegKey[i].OpenSubKey(regStrings[i]);
                    if (i == regStrings.Length - 1)
                    {
                        result = (string)RegKey[i + 1].GetValue(KeyName, DefaultValue);
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
            
        }  

    }
}
