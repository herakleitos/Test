using System;
namespace Chaint.Common.Core.Const
{
    public static class Const_AppConfigFilePath
    {
        private static string BaseDir =
            AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\";
        //针对Sqlite配置文件对应的路径
        public static string Sqlite = BaseDir + "AppConfig.db";
        //针对Access配置文件对应的路径    
        public static string Access = BaseDir + "AppConfig.mdb";
        //INI文件格式对应的路径
        public static string Ini = BaseDir + "AppConfig.ini";
        //XML文件内容对应的路径
        public static string XML = BaseDir + "AppConfig.xml";

    }
    public static class Const_DevicesConfigFilePath
    {
        private static string BaseDir =
            AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\";
        //针对Sqlite配置文件对应的路径
        public static string Sqlite = BaseDir + "DevicesConfig.db";
        //针对Access配置文件对应的路径    
        public static string Access = BaseDir + "DevicesConfig.mdb";
        //INI文件格式对应的路径
        public static string Ini = BaseDir + "DevicesConfig.ini";
        //XML文件内容对应的路径
        public static string XML = BaseDir + "DevicesConfig.xml";

    }
    public static class Const_TempletFilePath
    {
        private static string BaseDir =
            AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Templet\\";
        public static string DefaultReportTempletFolder = BaseDir;
        //默认的库区计划打印模板
        public static string DataPlanReportTemplet = BaseDir + "计划数据模板.repx";
        //默认的库区初始化打印模板
        public static string DataInitReportTemplet = BaseDir + "初始化数据模板.repx";
        //默认的库区数据管理打印模板
        public static string DataManageReportTemplet = BaseDir + "库区数据模板.repx";

    }
}
