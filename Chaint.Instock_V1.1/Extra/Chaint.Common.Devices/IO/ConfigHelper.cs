using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Configuration; //��Ҫ�������System.Configuration.dll
using System.Web;


/*-----------------------------------------------------------------------------------
 * ����: Chaint.IT
 * 
 * ����ʱ��: 2015-09-28
 * 
 * ��������: 
 *      ��Ҫ���ڶ�ִ���ļ��µ�Config�ļ��Ķ�д����
 *          ConfigHelper co = new ConfigHelper(ConfigType.ExeConfig); 
            co.AddAppSetting("Font-Size", "9"); 
            co.AddAppSetting("WebSite", "http://www.chaint.net"); 
            co.AddConnectionString("Connection", "test");  

 ------------------------------------------------------------------------------------*/

namespace Chaint.Common.Devices.IO
{
    /// <summary> 
    /// ˵����Config�ļ�����ö�٣� 
    /// �ֱ�Ϊasp.net��վ��config�ļ���WinForm��config�ļ� 
    /// ���ߣ�����ܹ�
    /// </summary> 
    public enum ConfigType
    {
        /// <summary> 
        /// asp.net��վ��config�ļ� 
        /// </summary> 
        WebConfig = 1,
        /// <summary> 
        /// WindowsӦ�ó����config�ļ� 
        /// </summary> 
        ExeConfig = 2
    }

    /// <summary> 
    /// ˵����������Ҫ����Գ��������ļ�(.config)�����޸ĵ��࣬ 
    /// ���Զ���վ��Ӧ�ó���������ļ������޸� 
    /// </summary> 
    public class ConfigHelper
    {
        private Configuration config;
        private string configPath;
        private ConfigType configType;


        /// <summary> 
        /// ��Ӧ�������ļ� 
        /// </summary> 
        public Configuration Configuration
        {
            get { return config; }
            set { config = value; }
        }
        /// <summary> 
        /// ���캯�� 
        /// </summary> 
        /// <param name="configType">.config�ļ������ͣ�ֻ������վ�����ļ�����Ӧ�ó��������ļ�</param> 
        public ConfigHelper(ConfigType configType)
        {
            this.configType = configType;
            if (configType == ConfigType.ExeConfig)
            {
                configPath = Application.ExecutablePath; //AppDomain.CurrentDomain.BaseDirectory; 
            }
            else
            {
                configPath = HttpContext.Current.Request.ApplicationPath;
            }
            Initialize();
        }
        /// <summary> 
        /// ���캯�� 
        /// </summary> 
        /// <param name="path">.config�ļ���λ��</param> 
        /// <param name="type">.config�ļ������ͣ�ֻ������վ�����ļ�����Ӧ�ó��������ļ�</param> 
        public ConfigHelper(string configPath, ConfigType configType)
        {
            this.configPath = configPath;
            this.configType = configType;
            Initialize();
        }
        //ʵ����configuration,���������ļ����͵Ĳ�ͬ���ֱ��ȡ�˲�ͬ��ʵ�������� 
        private void Initialize()
        {
            //�����WinFormӦ�ó���������ļ� 
            if (configType == ConfigType.ExeConfig)
            {
                config = System.Configuration.ConfigurationManager.OpenExeConfiguration(configPath);
            }
            else//WebForm�������ļ� 
            {
                config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(configPath);
            }
        }

        /// <summary> 
        /// ���Ӧ�ó������ýڵ㣬����Ѿ����ڴ˽ڵ㣬����޸ĸýڵ��ֵ 
        /// </summary> 
        /// <param name="key">�ڵ�����</param> 
        /// <param name="value">�ڵ�ֵ</param> 
        public void AddAppSetting(string key, string value)
        {
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] == null)//��������ڴ˽ڵ㣬����� 
            {
                appSetting.Settings.Add(key, value);
            }
            else//������ڴ˽ڵ㣬���޸� 
            {
                ModifyAppSetting(key, value);
            }
            config.Save();
        }
        /// <summary> 
        /// ������ݿ������ַ����ڵ㣬����Ѿ����ڴ˽ڵ㣬����޸ĸýڵ��ֵ 
        /// </summary> 
        /// <param name="key">�ڵ�����</param> 
        /// <param name="value">�ڵ�ֵ</param> 
        public void AddConnectionString(string key, string connectionString)
        {
            ConnectionStringsSection connectionSetting = (ConnectionStringsSection)config.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] == null)//��������ڴ˽ڵ㣬����� 
            {
                ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(key, connectionString);
                connectionSetting.ConnectionStrings.Add(connectionStringSettings);
            }
            else//������ڴ˽ڵ㣬���޸� 
            {
                ModifyConnectionString(key, connectionString);
            }
            config.Save();
        }

        /// <summary> 
        /// �޸�Ӧ�ó������ýڵ㣬��������ڴ˽ڵ㣬�����Ӵ˽ڵ㼰��Ӧ��ֵ 
        /// </summary> 
        /// <param name="key">�ڵ�����</param> 
        /// <param name="value">�ڵ�ֵ</param> 
        public void ModifyAppSetting(string key, string newValue)
        {
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] != null)//������ڴ˽ڵ㣬���޸� 
            {
                appSetting.Settings[key].Value = newValue;
            }
            else//��������ڴ˽ڵ㣬����� 
            {
                AddAppSetting(key, newValue);
            }
            config.Save();
        }

        /// <summary> 
        /// �޸����ݿ������ַ����ڵ㣬��������ڴ˽ڵ㣬�����Ӵ˽ڵ㼰��Ӧ��ֵ 
        /// </summary> 
        /// <param name="key">�ڵ�����</param> 
        /// <param name="value">�ڵ�ֵ</param> 
        public void ModifyConnectionString(string key, string connectionString)
        {
            ConnectionStringsSection connectionSetting = (ConnectionStringsSection)config.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] != null)//������ڴ˽ڵ㣬���޸� 
            {
                connectionSetting.ConnectionStrings[key].ConnectionString = connectionString;
            }
            else//��������ڴ˽ڵ㣬����� 
            {
                AddConnectionString(key, connectionString);
            }
            config.Save();
        }

        /// <summary> 
        /// �����������޸� 
        /// </summary> 
        public void Save()
        {
            config.Save();
        }

        /// <summary>
        /// ��ȡAppSettings�ڵ��ж�Ӧ����ֵ
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string GetValue(string strKey)
        {
            return ConfigurationManager.AppSettings[strKey];
        }

        /// <summary>
        /// ��ȡ�����ַ���
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string GetConnectString(string strKey)
        {
            return ConfigurationManager.ConnectionStrings[strKey].ConnectionString;
        }

    }

}
