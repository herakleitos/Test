using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Configuration; //需要添加引用System.Configuration.dll
using System.Web;


/*-----------------------------------------------------------------------------------
 * 作者: Chaint.IT
 * 
 * 创建时间: 2015-09-28
 * 
 * 功能描述: 
 *      主要用于对执行文件下的Config文件的读写操作
 *          ConfigHelper co = new ConfigHelper(ConfigType.ExeConfig); 
            co.AddAppSetting("Font-Size", "9"); 
            co.AddAppSetting("WebSite", "http://www.chaint.net"); 
            co.AddConnectionString("Connection", "test");  

 ------------------------------------------------------------------------------------*/

namespace Chaint.Common.Devices.IO
{
    /// <summary> 
    /// 说明：Config文件类型枚举， 
    /// 分别为asp.net网站的config文件和WinForm的config文件 
    /// 作者：借鉴周公
    /// </summary> 
    public enum ConfigType
    {
        /// <summary> 
        /// asp.net网站的config文件 
        /// </summary> 
        WebConfig = 1,
        /// <summary> 
        /// Windows应用程序的config文件 
        /// </summary> 
        ExeConfig = 2
    }

    /// <summary> 
    /// 说明：本类主要负责对程序配置文件(.config)进行修改的类， 
    /// 可以对网站和应用程序的配置文件进行修改 
    /// </summary> 
    public class ConfigHelper
    {
        private Configuration config;
        private string configPath;
        private ConfigType configType;


        /// <summary> 
        /// 对应的配置文件 
        /// </summary> 
        public Configuration Configuration
        {
            get { return config; }
            set { config = value; }
        }
        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="configType">.config文件的类型，只能是网站配置文件或者应用程序配置文件</param> 
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
        /// 构造函数 
        /// </summary> 
        /// <param name="path">.config文件的位置</param> 
        /// <param name="type">.config文件的类型，只能是网站配置文件或者应用程序配置文件</param> 
        public ConfigHelper(string configPath, ConfigType configType)
        {
            this.configPath = configPath;
            this.configType = configType;
            Initialize();
        }
        //实例化configuration,根据配置文件类型的不同，分别采取了不同的实例化方法 
        private void Initialize()
        {
            //如果是WinForm应用程序的配置文件 
            if (configType == ConfigType.ExeConfig)
            {
                config = System.Configuration.ConfigurationManager.OpenExeConfiguration(configPath);
            }
            else//WebForm的配置文件 
            {
                config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(configPath);
            }
        }

        /// <summary> 
        /// 添加应用程序配置节点，如果已经存在此节点，则会修改该节点的值 
        /// </summary> 
        /// <param name="key">节点名称</param> 
        /// <param name="value">节点值</param> 
        public void AddAppSetting(string key, string value)
        {
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] == null)//如果不存在此节点，则添加 
            {
                appSetting.Settings.Add(key, value);
            }
            else//如果存在此节点，则修改 
            {
                ModifyAppSetting(key, value);
            }
            config.Save();
        }
        /// <summary> 
        /// 添加数据库连接字符串节点，如果已经存在此节点，则会修改该节点的值 
        /// </summary> 
        /// <param name="key">节点名称</param> 
        /// <param name="value">节点值</param> 
        public void AddConnectionString(string key, string connectionString)
        {
            ConnectionStringsSection connectionSetting = (ConnectionStringsSection)config.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] == null)//如果不存在此节点，则添加 
            {
                ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(key, connectionString);
                connectionSetting.ConnectionStrings.Add(connectionStringSettings);
            }
            else//如果存在此节点，则修改 
            {
                ModifyConnectionString(key, connectionString);
            }
            config.Save();
        }

        /// <summary> 
        /// 修改应用程序配置节点，如果不存在此节点，则会添加此节点及对应的值 
        /// </summary> 
        /// <param name="key">节点名称</param> 
        /// <param name="value">节点值</param> 
        public void ModifyAppSetting(string key, string newValue)
        {
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] != null)//如果存在此节点，则修改 
            {
                appSetting.Settings[key].Value = newValue;
            }
            else//如果不存在此节点，则添加 
            {
                AddAppSetting(key, newValue);
            }
            config.Save();
        }

        /// <summary> 
        /// 修改数据库连接字符串节点，如果不存在此节点，则会添加此节点及对应的值 
        /// </summary> 
        /// <param name="key">节点名称</param> 
        /// <param name="value">节点值</param> 
        public void ModifyConnectionString(string key, string connectionString)
        {
            ConnectionStringsSection connectionSetting = (ConnectionStringsSection)config.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] != null)//如果存在此节点，则修改 
            {
                connectionSetting.ConnectionStrings[key].ConnectionString = connectionString;
            }
            else//如果不存在此节点，则添加 
            {
                AddConnectionString(key, connectionString);
            }
            config.Save();
        }

        /// <summary> 
        /// 保存所作的修改 
        /// </summary> 
        public void Save()
        {
            config.Save();
        }

        /// <summary>
        /// 获取AppSettings节点中对应属性值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string GetValue(string strKey)
        {
            return ConfigurationManager.AppSettings[strKey];
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string GetConnectString(string strKey)
        {
            return ConfigurationManager.ConnectionStrings[strKey].ConnectionString;
        }

    }

}
