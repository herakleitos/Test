using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Threading;
using System.Drawing;
using System.Reflection;
using System.Net;
using System.Windows.Forms;
using DataModel;
using CTSocket;

namespace CTWH.Common
{
    public partial class Utils
    {

        //FilePath
        public static string FilePath_txtImajeJetReply = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\ImajeJetReply.txt";
        
        public static string FilePath_txtServiceLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\ServiceLog.txt";
        public static string FilePath_txtErroLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\ErrorLog.txt";
        
        public static string FilePath_txtMSSQLLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\MSSQLLog.txt";
        public static string FilePath_txtMySQLLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\MySQLLog.txt";
        public static string FilePath_txtOracleSQLLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\OracleSQLLog.txt";//SAP
        
        public static string FilePath_txtDEMAGLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\DEMAGLog.txt";
        public static string FilePath_txtKONELog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\KONELog.txt";

        public static string FilePath_txtMetsoLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\MetsoLog.txt";
        public static string FilePath_txtMSKLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\MSKLog.txt";

        public static string FilePath_txtSheetScanPrintLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\SheetScanPrintLog.txt";
        public static string FilePath_txtWHScanLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\WHScanLog.txt";

        public static string FilePath_txtLogClearLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\LogClearLog.txt";

        public static string FilePath_txtERPLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\ERPLog.txt";
        public static string FilePath_txtAlarmLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\AlarmLog.txt";
        public static string FilePath_txtPLCLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\PLCLog.txt";
        public static string FilePath_txtPLCReadServerLog = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\PLCReadServerLog.txt";

        public static string FilePath_ReportDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Data\\Report";
        
        public static string FilePath_WHDetailLayoutDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\WHDetailLayout\\";
        
        public static string FilePath_DataDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Data\\";
        public static string FilePath_TempDB = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Data\\temp"; //"TempDB";
        public static string FilePath_AlarmLogDB = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Data\\AlarmLogdb";
        public static string FilePath_ImagesDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Data\\Images\\";



        public static string FilePath_AppConfig = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\app.ini";


        public static string FilePath_AppConfig_Bak = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\app_bak.ini";


        public static string FilePath_JetSettingsDataTemp = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\JetSettingsDataTemp.xml";
        public static string FilePath_JetSettings = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\JetSettings.xml";
                
        public static string FilePath_InitPageTable = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\InitPageData.xml";

        public static string FilePath_InitPagePalletTable = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\PalletInitPageData.xml";

        //Paper_DeWeightRule
        public static string FilePath_Paper_DeWeightRule = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\Paper_DeWeightRule.xml";

        public static string FilePath_PositionRules = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\PositionRules.xml";
        
        public static string FilePath_AppConfigBackup = "";//AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\app.config";
        
        public static string FilePath_App1Config = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\app1.config";
        public static string FilePath_App2Config = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\app2.config";
        public static string FilePath_App3Config = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\app3.config";
        public static string FilePath_App4Config = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\app4.config";
        public static string FilePath_App5Config = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\app5.config";
        
        public static string FilePath_Boot = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "boot.xml";

        public static string FilePath_AutoUpdateExe = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "AutoUpdate.exe";
     
      
        
       
    } 
}
