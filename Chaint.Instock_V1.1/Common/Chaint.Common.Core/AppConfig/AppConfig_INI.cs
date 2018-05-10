using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Const;
namespace Chaint.Common.Core.AppConfig
{
    public class AppConfig_INI
    {
        private string FilePath = string.Empty;
        public const int MaxSectionSize = 32767; // 32 KB
        public Enums_AppConfigType ConfigType
        {
            get
            {
                return Enums_AppConfigType.INI;
            }
        }
        public AppConfig_INI(string filePath)
        {
            if (filePath != "")
            {
                if (!File.Exists(filePath)) File.Create(filePath);
                FilePath = filePath;
            }
            else
            {
                FilePath = Const_AppConfigFilePath.Ini;
            }
        }


        public string GetValue(string strSectionName, string strKeyName, string strDefaultValue)
        {
            return GetString(strSectionName, strKeyName, strDefaultValue);
        }

        public bool SetValue(string strSectionName, string strKeyName, string strValue)
        {
            return WriteValueInternal(strSectionName, strKeyName, strValue);
        }
        /// <summary>
        /// Writes a <see cref="T:System.String"/> value to the ini file.
        /// </summary>
        /// <param name="sectionName">The name of the section to write to .</param>
        /// <param name="keyName">The name of the key to write to.</param>
        /// <param name="value">The string value to write</param>
        /// <exception cref="T:System.ComponentModel.Win32Exception">
        /// The write failed.
        /// </exception>
        private bool WriteValueInternal(string sectionName, string keyName, string value)
        {
            bool retValue = NativeMethods.WritePrivateProfileString(sectionName, keyName, value, FilePath);
            if (!retValue)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return retValue;
        }
        /// <summary>
        /// Gets the value of a setting in an ini file as a <see cref="T:System.String"/>.
        /// </summary>
        /// <param name="sectionName">The name of the section to read from.</param>
        /// <param name="keyName">The name of the key in section to read.</param>
        /// <param name="defaultValue">The default value to return if the key
        /// cannot be found.</param>
        /// <returns>The value of the key, if found.  Otherwise, returns 
        /// <paramref name="defaultValue"/></returns>
        /// <remarks>
        /// The retreived value must be less than 32KB in length.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="keyName"/> are 
        /// a null reference  (Nothing in VB)
        /// </exception>
        private string GetString(string sectionName, string keyName, string defaultValue)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            StringBuilder retval = new StringBuilder(MaxSectionSize);

            NativeMethods.GetPrivateProfileString(sectionName,
                                                  keyName,
                                                  defaultValue,
                                                  retval,
                                                  MaxSectionSize,
                                                  FilePath);

            return retval.ToString();
        }
        #region P/Invoke declares

        /// <summary>
        /// A static class that provides the win32 P/Invoke signatures 
        /// used by this class.
        /// </summary>
        /// <remarks>
        /// Note:  In each of the declarations below, we explicitly set CharSet to 
        /// Auto.  By default in C#, CharSet is set to Ansi, which reduces 
        /// performance on windows 2000 and above due to needing to convert strings
        /// from Unicode (the native format for all .Net strings) to Ansi before 
        /// marshalling.  Using Auto lets the marshaller select the Unicode version of 
        /// these functions when available.
        /// </remarks>
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
                                                                   uint nSize,
                                                                   string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName,
                                                              string lpKeyName,
                                                              string lpDefault,
                                                              StringBuilder lpReturnedString,
                                                              int nSize,
                                                              string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName,
                                                              string lpKeyName,
                                                              string lpDefault,
                                                              [In, Out] char[] lpReturnedString,
                                                              int nSize,
                                                              string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileString(string lpAppName,
                                                             string lpKeyName,
                                                             string lpDefault,
                                                             IntPtr lpReturnedString,
                                                             uint nSize,
                                                             string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileInt(string lpAppName,
                                                          string lpKeyName,
                                                          int lpDefault,
                                                          string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileSection(string lpAppName,
                                                              IntPtr lpReturnedString,
                                                              uint nSize,
                                                              string lpFileName);

            //We explicitly enable the SetLastError attribute here because
            // WritePrivateProfileString returns errors via SetLastError.
            // Failure to set this can result in errors being lost during 
            // the marshal back to managed code.
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool WritePrivateProfileString(string lpAppName,
                                                                string lpKeyName,
                                                                string lpString,
                                                                string lpFileName);


        }
        #endregion
    }
}
