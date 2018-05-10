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
        //Metso包装机通信服务
        private static bool IsFirstLoadServiceParaMetsoRollWrap = true;
        private static DataType.ServiceParameter _ServiceParaMetsoRollWrap;
        public static DataType.ServiceParameter ServiceParaMetsoRollWrap
        {
            get
            {
                if (IsFirstLoadServiceParaMetsoRollWrap)
                {
                    try
                    { 
                        //_ServiceParaMetsoRollWrap = new DataType.ServiceParameter(GetAppSettings("ServiceParaMetsoRollWrap"));
                        _ServiceParaMetsoRollWrap = new DataType.ServiceParameter( GetAppSettings("ServiceParaMetsoRollWrap"));
                    }
                    catch { }
                    IsFirstLoadServiceParaMetsoRollWrap = false;
                }
                return _ServiceParaMetsoRollWrap;
            }
            set
            {
                _ServiceParaMetsoRollWrap = value;
                try
                {
                    SetAppSettings("ServiceParaMetsoRollWrap", _ServiceParaMetsoRollWrap.GetParameterString());
                }
                catch { }
            }
        }
        //MSK包装机通信服务
        private static bool IsFirstLoadServiceParaMSKPalletWrap = true;
        private static DataType.ServiceParameter _ServiceParaMSKPalletWrap;
        public static DataType.ServiceParameter ServiceParaMSKPalletWrap
        {
            get
            {
                if (IsFirstLoadServiceParaMSKPalletWrap)
                {
                    try
                    {
                        _ServiceParaMSKPalletWrap = new DataType.ServiceParameter(GetAppSettings("ServiceParaMSKPalletWrap"));
                    }
                    catch { }
                    IsFirstLoadServiceParaMSKPalletWrap = false;
                }
                return _ServiceParaMSKPalletWrap;
            }
            set
            {
                _ServiceParaMSKPalletWrap = value;
                try
                {
                    SetAppSettings("ServiceParaMSKPalletWrap", _ServiceParaMSKPalletWrap.GetParameterString());
                }
                catch { }
            }
        }
        //Chaint通信日志清理服务
        private static bool IsFirstLoadServiceParaLogClear = true;
        private static DataType.ServiceParameter _ServiceParaLogClear;
        public static DataType.ServiceParameter ServiceParaLogClear
        {
            get
            {
                if (IsFirstLoadServiceParaLogClear)
                {
                    try
                    {
                       // _ServiceParaLogClear = new DataType.ServiceParameter(GetAppSettings("ServiceParaLogClear"));
                        _ServiceParaLogClear = new DataType.ServiceParameter( GetAppSettings("ServiceParaLogClear"));
                    }
                    catch { }
                    IsFirstLoadServiceParaLogClear = false;
                }
                return _ServiceParaLogClear;
            }
            set
            {
                _ServiceParaLogClear = value;
                try
                {
                    SetAppSettings("ServiceParaLogClear", _ServiceParaLogClear.GetParameterString());
                }
                catch { }
            }
        }

        //切纸扫描打印
        private static bool IsFirstLoadServiceParaSheetScanPrint = true;
        private static DataType.ServiceParameter _ServiceParaSheetScanPrint;
        public static DataType.ServiceParameter ServiceParaSheetScanPrint
        {
            get
            {
                if (IsFirstLoadServiceParaSheetScanPrint)
                {
                    try
                    {
                        _ServiceParaSheetScanPrint = new DataType.ServiceParameter(GetAppSettings("ServiceParaSheetScanPrint"));
                    }
                    catch { }
                    IsFirstLoadServiceParaSheetScanPrint = false;
                }
                return _ServiceParaSheetScanPrint;
            }
            set
            {
                _ServiceParaSheetScanPrint = value;
                try
                {
                    SetAppSettings("ServiceParaSheetScanPrint", _ServiceParaSheetScanPrint.GetParameterString());
                }
                catch { }
            }
        }
        
        //仓库出入库扫描程序
        private static bool IsFirstLoadServiceParaWHScan = true;
        private static DataType.ServiceParameter _ServiceParaWHScan;
        public static DataType.ServiceParameter ServiceParaWHScan
        {
            get
            {
                if (IsFirstLoadServiceParaWHScan)
                {
                    try
                    {
                        _ServiceParaWHScan = new DataType.ServiceParameter(GetAppSettings("ServiceParaWHScan"));
                    }
                    catch { }
                    IsFirstLoadServiceParaWHScan = false;
                }
                return _ServiceParaWHScan;
            }
            set
            {
                _ServiceParaWHScan = value;
                try
                {
                    SetAppSettings("ServiceParaWHScan", _ServiceParaWHScan.GetParameterString());
                }
                catch { }
            }
        }

        //ERP通信服务程序
        private static bool IsFirstLoadServiceParaERPComunication = true;
        private static DataType.ServiceParameter _ServiceParaERPComunication;
        public static DataType.ServiceParameter ServiceParaERPComunication
        {
            get
            {
                if (IsFirstLoadServiceParaERPComunication)
                {
                    try
                    {
                        _ServiceParaERPComunication = new DataType.ServiceParameter(GetAppSettings("ServiceParaERPComunication"));
                    }
                    catch { }
                    IsFirstLoadServiceParaERPComunication = false;
                }
                return _ServiceParaERPComunication;
            }
            set
            {
                _ServiceParaERPComunication = value;
                try
                {
                    SetAppSettings("ServiceParaERPComunication", _ServiceParaERPComunication.GetParameterString());
                }
                catch { }
            }
        }


        public static string[] GetAllServiceParameter()
        {
            return MyAppConfig.GetEntryNames("serviceParameters");        
        }

        public static string GetserviceParameters(string key)
        {
            return GetConfigValue("serviceParameters", key);
        }







        //StorageService.exe
        public static int SelectRowsMax = 5000;

        public static int SelectERPDataRowsMax = 2000;

        //Format
        public static string FormatDate = "yyyyMMdd";
        public static string FormatTime = "HHmmss";

        //public static string PLCCommunicationDetectFlag = "PLCServerDetect";
        public static string ServerDetectFlag = "chaint";


        public static string SplitDot = "."; 
 

        private static Profile.Config myappconfig = null;
        public static Profile.Config MyAppConfig
        {
            get
            {
                if (myappconfig == null)
                {
                    //ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    //fileMap.ExeConfigFilename = FilePath_AppConfig;
                    //myappconfig = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                    myappconfig = new Profile.Config(FilePath_AppConfig);
                }
                return myappconfig;
            }
        }

        private static Profile.Config myappconfigbak = null;
        public static Profile.Config MyAppConfigBak
        {
            get
            {
                if (myappconfigbak == null)
                {

                    myappconfigbak = new Profile.Config(FilePath_AppConfig_Bak);
                }
                return myappconfigbak;
            }
        }         



        //private static SQLiteHelper sqliteaccess = new SQLiteHelper();  
        private static Profile.Registry myappregistry = null;
        public static Profile.Registry MyAppRegistry
        {
            get
            {
                if (myappregistry == null)
                {
                    myappregistry = new Profile.Registry(Microsoft.Win32.Registry.LocalMachine, @"Software\Chaint\ChaintApp");
  
                }
                return myappregistry;
            }

        }
     

        //parameter
        public static bool AllowReStorage = true;//是否允许中仓重复入库

        public static bool IsFormClosing = false;

        //SQL       
        private static string _SQLServer = "";
        public static string SQLServer
        {
            get
            {
                if (_SQLServer == "")
                {
                    System.Data.SqlClient.SqlConnectionStringBuilder sbconnection = new System.Data.SqlClient.SqlConnectionStringBuilder(Utils.SQLConnectionString);

                    _SQLServer = sbconnection.DataSource;
                }
                return _SQLServer;
            }           
        }
        private static string _SQLDataBase = "";
        public static string SQLDataBase
        {
            get
            {
                if (_SQLDataBase == "")
                {
                    System.Data.SqlClient.SqlConnectionStringBuilder sbconnection = new System.Data.SqlClient.SqlConnectionStringBuilder(Utils.SQLConnectionString);

                    _SQLDataBase = sbconnection.InitialCatalog;
                }
                return _SQLDataBase;
            }
        }
        private static string _SQLUserID = "";
        public static string SQLUserID
        {
            get
            {
                if (_SQLUserID == "")
                {
                    System.Data.SqlClient.SqlConnectionStringBuilder sbconnection = new System.Data.SqlClient.SqlConnectionStringBuilder(Utils.SQLConnectionString);

                    _SQLUserID = sbconnection.UserID;
                }
                return _SQLUserID;
            }
        }
        private static string _SQLPassword = "";
        public static string SQLPassword
        {
            get
            {
                if (_SQLPassword == "")
                {
                    System.Data.SqlClient.SqlConnectionStringBuilder sbconnection = new System.Data.SqlClient.SqlConnectionStringBuilder(Utils.SQLConnectionString);

                    _SQLPassword = sbconnection.Password;
                }
                return _SQLPassword;         
            }
        }
        private static MSSQL.WMSAccess _WMSSqlAccess = null;
        public static MSSQL.WMSAccess WMSSqlAccess
        {
            get
            {
                if (_WMSSqlAccess == null)
                {
                    _WMSSqlAccess = new MSSQL.WMSAccess();
                }
                return _WMSSqlAccess;
            }
        }
        private static MSSQL.MSSQLAccess _MSSqlAccess = null;
        public static MSSQL.MSSQLAccess MSSqlAccess
        {
            get
            {
                if (_MSSqlAccess == null)
                {
                    _MSSqlAccess = new MSSQL.MSSQLAccess();
                }
                return _MSSqlAccess;
            }
        }
        public static MSSQL.MSSQLAccess WebMSSqlAccess
        {
            get
            {
                if (_MSSqlAccess == null)
                {
                    _MSSqlAccess = new MSSQL.MSSQLAccess(Utils.WebSQLConnectionString);
                }
                return _MSSqlAccess;
            }
        }

        //private static NPG.NpgSQLAccess _NpgAccess = null;
        //public static NPG.NpgSQLAccess NpgAccess
        //{
        //    get
        //    {
        //        if (_NpgAccess == null)
        //        {
        //            _NpgAccess = new NPG.NpgSQLAccess();

        //        }
        //        return _NpgAccess;
        //    }
        //}

        //private static MySQL.MySQLAccess _MySqlAccess = null;
        //public static MySQL.MySQLAccess MySqlAccess
        //{
        //    get
        //    {
        //        if (_MySqlAccess == null)
        //        {
        //            _MySqlAccess = new Common.MySQL.MySQLAccess();

        //        }
        //        return _MySqlAccess;
        //    }
        //}

        //private static Oracle.OracleSQLAccess _OracleAccess = null;
        //public static Oracle.OracleSQLAccess OracleAccess
        //{
        //    get
        //    {
        //        if (_OracleAccess == null)
        //        {
        //            _OracleAccess = new Oracle.OracleSQLAccess();

        //        }
        //        return _OracleAccess;
        //    }
        //}


        private static PubFunction _PF = null;
        public static PubFunction PF
        {
            get
            {
                if (_PF == null)
                {
                    _PF = new PubFunction();

                }
                return _PF;
            }
        }

        private static PLCSokcet.PLCReadClient _PLCReadClient = null;
        public static PLCSokcet.PLCReadClient PLCReadClient
        {
            get
            {
                if (_PLCReadClient == null)
                {
                    _PLCReadClient = new PLCSokcet.PLCReadClient();
                   // _PLCReadClient.InitPLCClient();

                }
                return _PLCReadClient;
            }
        }
      
        //PaperUser
        public static string LoginUserName = "";
        public static string LoginUserType = "";
        public static string LoginUserPassword = "";
        //public static string LoginUserPrivilige = "";
        public static string LoginUserShift = "";
        private static string _LoginMachineID = "";
        public static string LoginMachineID {
            get
            {
                if (_LoginMachineID == "")
                {
                    try
                    {
                        _LoginMachineID = GetAppSettings("LoginMachineID");
                    }
                    catch { }
                }
                return _LoginMachineID;
            }
            set
            {
                _LoginMachineID = value.ToString();
                try
                {
                    SetAppSettings("LoginMachineID", value);
                }
                catch { }
            }
        
        }

        public static bool  LoginIsOK = false;
        public static MainDS.PaperUserPriviligeDataTable LoginUserPrivilige = new MainDS.PaperUserPriviligeDataTable();
        //public static Common.DataType.PaperUserPrivilige LoginPaperUserPrivilige;

        public static char PaperMachineCode = '1';
        public static char PaperWinderCode = '1';

        private static string _LastLoginUser = "";
        public static string LastLoginUser
        {
            get
            {
                if (_LastLoginUser == "")
                {
                    try
                    {
                        _LastLoginUser = GetAppSettings("LastLoginUser");
                    }
                    catch { }
                }
                return _LastLoginUser;
            }
            set
            {
                _LastLoginUser = value.ToString();
                try
                {
                    SetAppSettings("LastLoginUser", value);
                }
                catch { }
            }
        }


        private static string _MESSQLConnectString = "";
        public static string MESSQLConnectString
        {
            get
            {
                if (_MESSQLConnectString == "")
                {
                    try
                    {
                        //_SQLConnectionString = MyAppConfig.ConnectionStrings.ConnectionStrings["SQLConnectionString"].ConnectionString;
                        _MESSQLConnectString = GetconnectionStrings("MESSQLConnectString");
                    }
                    catch { }
                }
                return _MESSQLConnectString;
            }
            set
            {
                _MESSQLConnectString = value;
                try
                {
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Remove("SQLConnectionString");
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("SQLConnectionString", value));
                    //MyAppConfig.Save(ConfigurationSaveMode.Modified);
                    SetAppSettings("connectionStrings", "MESSQLConnectString", _MESSQLConnectString);
                }
                catch { }
            }
        }

        private static string _ZZSQLConnctionString = "";
        public static string ZZSQLConnctionString
        {
            get
            {
                if (_ZZSQLConnctionString == "")
                {
                    try
                    {
                        //_SQLConnectionString = MyAppConfig.ConnectionStrings.ConnectionStrings["SQLConnectionString"].ConnectionString;
                        _ZZSQLConnctionString = GetconnectionStrings("ZZSQLConnctionString");
                    }
                    catch { }
                }
                return _ZZSQLConnctionString;
            }
            set
            {
                _ZZSQLConnctionString = value;
                try
                {
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Remove("SQLConnectionString");
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("SQLConnectionString", value));
                    //MyAppConfig.Save(ConfigurationSaveMode.Modified);
                    SetAppSettings("connectionStrings", "ZZSQLConnctionString", _ZZSQLConnctionString);
                }
                catch { }
            }
        }

        private static string _SQLConnectionString = "";
        public static string SQLConnectionString
        {
            get
            {
                if (_SQLConnectionString == "")
                {
                    try
                    {
                        //_SQLConnectionString = MyAppConfig.ConnectionStrings.ConnectionStrings["SQLConnectionString"].ConnectionString;
                        _SQLConnectionString = GetconnectionStrings( "SQLConnectionString");
                    }
                    catch { }
                }
                return _SQLConnectionString;
            }
            set
            {
                _SQLConnectionString = value;
                try
                {
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Remove("SQLConnectionString");
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("SQLConnectionString", value));
                    //MyAppConfig.Save(ConfigurationSaveMode.Modified);
                    SetAppSettings("connectionStrings", "SQLConnectionString", _SQLConnectionString);
                }
                catch { }
            }
        }
        public static string WebSQLConnectionString
        {
            get
            {
                if (_SQLConnectionString == "")
                {
                    try
                    {
                        _SQLConnectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
                    }
                    catch { }
                }
                return _SQLConnectionString;
            }
        }

        private static string _MySqlConnectionString = "";


        public static string MySqlConnectionString
        {
            get
            {
                if (_MySqlConnectionString == "")
                {
                    try
                    {
                        //_MySqlConnectionString = MyAppConfig.ConnectionStrings.ConnectionStrings["MySqlConnectionString"].ConnectionString;
                        _MySqlConnectionString = GetconnectionStrings("MySqlConnectionString");
                    }
                    catch { }
                }
                return _MySqlConnectionString;
            }
            set
            {
                _MySqlConnectionString = value;
                try
                {
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Remove("MySqlConnectionString");
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("MySqlConnectionString", value));
                    //MyAppConfig.Save(ConfigurationSaveMode.Modified);

                    SetAppSettings("connectionStrings", "MySqlConnectionString", _MySqlConnectionString);
                }
                catch { }
            }
        }

        private static string _OracleConnectionString = "";
        public static string OracleConnectionString
        {
            get
            {
                if (_OracleConnectionString == "")
                {
                    try
                    {
                        //_OracleConnectionString = MyAppConfig.ConnectionStrings.ConnectionStrings["OracleConnectionString"].ConnectionString;
                        _OracleConnectionString = GetconnectionStrings("OracleConnectionString");
                    }
                    catch { }
                }
                return _OracleConnectionString;
            }
            set
            {
                _OracleConnectionString = value;
                try
                {
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Remove("OracleConnectionString");
                    //MyAppConfig.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("OracleConnectionString", value));
                    //MyAppConfig.Save(ConfigurationSaveMode.Modified);
                    SetAppSettings("connectionStrings", "OracleConnectionString", _OracleConnectionString);
                }
                catch { }
            }
        }

        private static string _ERPSQLConnectionString = "";
        public static string ERPSQLConnectionString
        {
            get
            {
                if (_ERPSQLConnectionString == "")
                {
                    try
                    {
                        _ERPSQLConnectionString = GetconnectionStrings( "ERPSQLConnectionString");
                    }
                    catch { }
                }
                return _ERPSQLConnectionString;
            }
            set
            {
                _ERPSQLConnectionString = value;
                try
                {
                    SetAppSettings("connectionStrings", "ERPSQLConnectionString", _ERPSQLConnectionString);
                }
                catch { }
            }
        }

        private static string _ERPSchemeName = "";
        public static string ERPSchemeName
        {
            get
            {
                if (_ERPSchemeName == "")
                {
                    try
                    {
                        _ERPSchemeName = GetconnectionStrings("ERPSchemeName");
                    }
                    catch { }
                }
                return _ERPSchemeName;
            }
            set
            {
                _ERPSchemeName = value;
                try
                {
                    SetAppSettings("connectionStrings", "ERPSchemeName", _ERPSchemeName);
                }
                catch { }
            }
        }


        //SheetScanPrint Service
        private static bool IsFirstLoadSocketParaSheetScanPrint = true;
        private static DataType.SocketParameter _SocketParaSheetScanPrint;
        public static DataType.SocketParameter SocketParaSheetScanPrint
        {
            get
            {
                if (IsFirstLoadSocketParaSheetScanPrint)
                {
                    try
                    {
                        _SocketParaSheetScanPrint = new DataType.SocketParameter(GetAppSettings("SocketParaSheetScanPrint"));
                    }
                    catch { }

                    IsFirstLoadSocketParaSheetScanPrint = false;
                }
                return _SocketParaSheetScanPrint;
            }

            set
            {
                _SocketParaSheetScanPrint = value;
                try
                {
                    SetAppSettings("SocketParaSheetScanPrint", _SocketParaSheetScanPrint.GetParameterString());
                }
                catch { }
            }
        }       
        
        //WHScan Service
        private static bool IsFirstLoadSocketParaWHScan = true;
        private static DataType.SocketParameter _SocketParaWHScan;
        public static DataType.SocketParameter SocketParaWHScan
        {
            get
            {
                if (IsFirstLoadSocketParaWHScan)
                {
                    try
                    {
                        _SocketParaWHScan = new DataType.SocketParameter(GetAppSettings("SocketParaWHScan"));
                    }
                    catch { }

                    IsFirstLoadSocketParaWHScan = false;
                }
                return _SocketParaWHScan;
            }

            set
            {
                _SocketParaWHScan = value;
                try
                {
                    SetAppSettings("SocketParaWHScan", _SocketParaWHScan.GetParameterString());
                }
                catch { }
            }
        }       
 

        //PB50 printer
        private static bool IsFirstLoadSocketParaPB50 = true;
        private static DataType.SocketParameter _SocketParaPB50;
        public static DataType.SocketParameter SocketParaPB50
        {
            get
            {
                if (IsFirstLoadSocketParaPB50)
                {
                    try
                    {
                        _SocketParaPB50 = new DataType.SocketParameter(GetAppSettings("SocketParaPB50"));
                    }
                    catch { }

                    IsFirstLoadSocketParaPB50 = false;
                }

                return _SocketParaPB50;
            }

            set
            {
                _SocketParaPB50 = value;
                try
                {
                    SetAppSettings("SocketParaPB50", _SocketParaPB50.GetParameterString());

                }
                catch { }
            }
        } 
        //OPS290
        private static bool IsFirstLoadSocketParaOPS290 = true;
        private static DataType.SocketParameter _SocketParaOPS290;
        public static DataType.SocketParameter SocketParaOPS290
        {
            get
            {
                if (IsFirstLoadSocketParaOPS290)
                {
                    try
                    {
                        _SocketParaOPS290 = new DataType.SocketParameter(GetAppSettings("SocketParaOPS290"));
                    }
                    catch (Exception e){
                    string ss =e.Message;
                    }

                    IsFirstLoadSocketParaOPS290 = false;
                }

                return _SocketParaOPS290;
            }

            set
            {
                _SocketParaOPS290 = value;
                try
                {
                    SetAppSettings("SocketParaOPS290", _SocketParaOPS290.GetParameterString());

                }
                catch { }
            }
        } 

        //Metso RollWrap
        private static bool IsFirstLoadSocketParaMetsoRollWrap = true;
        private static DataType.SocketParameter _SocketParaMetsoRollWrap;
        public static DataType.SocketParameter SocketParaMetsoRollWrap
        {
            get
            {
                if (IsFirstLoadSocketParaMetsoRollWrap)
                {
                    try
                    {
                        _SocketParaMetsoRollWrap = new DataType.SocketParameter(GetAppSettings("SocketParaMetsoRollWrap"));
                    }
                    catch { }

                    IsFirstLoadSocketParaMetsoRollWrap = false;
                }
                return _SocketParaMetsoRollWrap;
            }

            set
            {
                _SocketParaMetsoRollWrap = value;
                try
                {
                    SetAppSettings("SocketParaMetsoRollWrap", _SocketParaMetsoRollWrap.GetParameterString());
                }
                catch { }
            }
        }
        //WH PDA SERVICE
        private static bool IsFirstLoadSocketParaPDA = true;
        private static DataType.SocketParameter _SocketParaPDA;
        public static DataType.SocketParameter SocketParaPDA
        {
            get
            {
                if (IsFirstLoadSocketParaPDA)
                {
                    try
                    {
                        _SocketParaPDA = new DataType.SocketParameter(GetAppSettings("SocketParaPDA"));
                    }
                    catch { }

                    IsFirstLoadSocketParaPDA = false;
                }
                return _SocketParaPDA;
            }

            set
            {
                _SocketParaPDA = value;
                try
                {
                    SetAppSettings("SocketParaPDA", _SocketParaPDA.GetParameterString());
                }
                catch { }
            }
        }       

        //MSK PalletWrap
        private static bool IsFirstLoadSocketParaMSKPalletWrap = true;
        private static DataType.SocketParameter _SocketParaMSKPalletWrap;
        public static DataType.SocketParameter SocketParaMSKPalletWrap
        {
            get
            {
                if (IsFirstLoadSocketParaMSKPalletWrap)
                {
                    try
                    {
                        _SocketParaMSKPalletWrap = new DataType.SocketParameter(GetAppSettings("SocketParaMSKPalletWrap"));
                    }
                    catch { }

                    IsFirstLoadSocketParaMSKPalletWrap = false;
                }
                return _SocketParaMSKPalletWrap;
            }

            set
            {
                _SocketParaMSKPalletWrap = value;
                try
                {
                    SetAppSettings("SocketParaMSKPalletWrap", _SocketParaMSKPalletWrap.GetParameterString());
                }
                catch { }
            }
        }       


        //MES
        private static bool IsFirstLoadSocketParaMESClient = true;
        private static DataType.SocketParameter _SocketParaMESClient;
        public static DataType.SocketParameter SocketParaMESClient
        {
            get
            {
                if (IsFirstLoadSocketParaMESClient)
                {
                    try
                    {
                        _SocketParaMESClient = new DataType.SocketParameter(GetAppSettings("SocketParaMESClient"));
                    }
                    catch { }

                    IsFirstLoadSocketParaMESClient = false;
                }

                return _SocketParaMESClient;

            }

            set
            {
                _SocketParaMESClient = value;
                try
                {
                    SetAppSettings("SocketParaMESClient", _SocketParaMESClient.GetParameterString());
                }
                catch { }
            }
        }       
        private static int onlyID = 0;
        public static int OnlyID
        {
            get
            {
                if (onlyID == Int32.MaxValue)
                    return 0;
                else
                    return onlyID++;

            }

        }
        
        //KONE
        private static bool IsFirstLoadSocketParaKONEClient = true;
        private static DataType.SocketParameter _SocketParaKONEClient;
        public static DataType.SocketParameter SocketParaKONEClient
        {
            get
            {
                if (IsFirstLoadSocketParaKONEClient)
                {
                    try
                    {

                        _SocketParaKONEClient = new DataType.SocketParameter(GetAppSettings("SocketParaKONEClient"));
                    }
                    catch { }

                    IsFirstLoadSocketParaKONEClient = false;
                }

                return _SocketParaKONEClient;

            }

            set
            {
                _SocketParaKONEClient = value;
                try
                {
                    SetAppSettings("SocketParaKONEClient", _SocketParaKONEClient.GetParameterString());
                    
                }
                catch { }
            }
        }

        private static bool IsFirstLoadSocketParaKONEServer = true;
        private static DataType.SocketParameter _SocketParaKONEServer;
        public static DataType.SocketParameter SocketParaKONEServer
        {
            get
            {
                if (IsFirstLoadSocketParaKONEServer)
                {
                    try
                    {

                        _SocketParaKONEServer = new DataType.SocketParameter(GetAppSettings("SocketParaKONEServer"));
                    }
                    catch { }

                    IsFirstLoadSocketParaKONEServer = false;
                }

                return _SocketParaKONEServer;

            }

            set
            {
                _SocketParaKONEServer = value;
                try
                {
                    SetAppSettings("SocketParaKONEServer", _SocketParaKONEServer.GetParameterString());
                    
                }
                catch { }
            }
        }

        public static char SocketDelimiterStorageCommunication = '>';
        private static bool IsFirstLoadSocketParaStorageCommunication = true;
        private static DataType.SocketParameter _SocketParaStorageCommunication;
        public static DataType.SocketParameter SocketParaStorageCommunication
        {
            get
            {
                if (IsFirstLoadSocketParaStorageCommunication)
                {
                    try
                    {
                        _SocketParaStorageCommunication = new DataType.SocketParameter(GetAppSettings("SocketParaStorageCommunication"));
                    }
                    catch { }
                    IsFirstLoadSocketParaStorageCommunication = false;
                }
                return _SocketParaStorageCommunication;
            }

            set
            {
                _SocketParaStorageCommunication = value;
                try
                {
                    SetAppSettings("SocketParaStorageCommunication", _SocketParaStorageCommunication.GetParameterString());
                }
                catch { }
            }
        }

        private static string _IsForceSTO = ""; //"0" ,"1"
        public static bool IsForceSTO
        {
            get
            {
                if (_IsForceSTO == "")
                {
                    try
                    {
                        _IsForceSTO = GetAppSettings("IsForceSTO");
                    }
                    catch { }
                }
                return _IsForceSTO.ToLower().Equals("true"); //默认除了"true"以外其他都为false
            }
            set
            {
                _IsForceSTO = value.ToString();
                try
                {
                    SetAppSettings("IsForceSTO", value.ToString());
                }
                catch { }
            }
        }


        private static int _RollWrapWidthPad = -1;
        public static int RollWrapWidthPad
        {
            get
            {
                if (_RollWrapWidthPad == -1)
                {
                    try
                    {
                        _RollWrapWidthPad = Convert.ToInt32(GetAppSettings("RollWrapWidthPad"));
                    }
                    catch {

                        Utils.RollWrapWidthPad = 10;//默认10mm
                    }
                }
                return _RollWrapWidthPad;
            }

            set
            {
                _RollWrapWidthPad = value;
                try
                {
                    SetAppSettings("RollWrapWidthPad", value.ToString());

                }
                catch { }
            }
        }



        private static int _BarcodeLength = -1;
        public static int BarcodeLength
        {
            get
            {
                if (_BarcodeLength == -1)
                {
                    try
                    {
                        _BarcodeLength = Convert.ToInt32(GetAppSettings("BarcodeLength"));
                    }
                    catch {
                        _BarcodeLength = 0;
                    }
                }
                return _BarcodeLength;
            }

            set
            {
                _BarcodeLength = value;
                try
                {
                    SetAppSettings("BarcodeLength", value.ToString());
                }
                catch { }
            }
        }
        
        private static bool IsFirstLoadScannerPara1 = true;
        private static DataType.ScannerParameter _ScannerPara1;
        public static DataType.ScannerParameter ScannerPara1
        {
            get
            {
                if (IsFirstLoadScannerPara1)
                {
                    try
                    {
                        _ScannerPara1 = new DataType.ScannerParameter(GetAppSettings("ScannerPara1"));
                    }
                    catch { }

                    IsFirstLoadScannerPara1 = false;
                }
                return _ScannerPara1;
            }
            set
            {
                _ScannerPara1 = value;
                try
                {
                    SetAppSettings("ScannerPara1", _ScannerPara1.GetParameterString());
                }
                catch { }
            }
        }

        private static bool IsFirstLoadScannerPara2 = true;
        private static DataType.ScannerParameter _ScannerPara2;
        public static DataType.ScannerParameter ScannerPara2
        {
            get
            {
                if (IsFirstLoadScannerPara2)
                {
                    try
                    {
                        _ScannerPara2 = new DataType.ScannerParameter(GetAppSettings("ScannerPara2"));
                    }
                    catch { }

                    IsFirstLoadScannerPara2 = false;
                }
                return _ScannerPara2;
            }
            set
            {
                _ScannerPara2 = value;
                try
                {
                    SetAppSettings("ScannerPara2", _ScannerPara2.GetParameterString());
                }
                catch { }
            }
        }

        private static bool IsFirstLoadScannerPara3 = true;
        private static DataType.ScannerParameter _ScannerPara3;
        public static DataType.ScannerParameter ScannerPara3
        {
            get
            {
                if (IsFirstLoadScannerPara3)
                {
                    try
                    {
                        _ScannerPara3 = new DataType.ScannerParameter(GetAppSettings("ScannerPara3"));
                    }
                    catch { }

                    IsFirstLoadScannerPara3 = false;
                }
                return _ScannerPara3;
            }
            set
            {
                _ScannerPara3 = value;
                try
                {
                    SetAppSettings("ScannerPara3", _ScannerPara3.GetParameterString());
                }
                catch { }
            }
        }

        //JET
        private static string _JetMSGName = "";
        public static string JetMSGName
        {
            get
            {
                if (_JetMSGName == "")
                {
                    try
                    {
                        _JetMSGName = GetAppSettings("JetMSGName");
                    }
                    catch { }
                }
                    //_JetMSGName = (ConfigRow == null || ConfigRow["JetMSGName"] == DBNull.Value) ? "" : ConfigRow["JetMSGName"].ToString();
                return _JetMSGName;
            }
            set 
            {
                
                  _JetMSGName = value.ToString();
                try
                {
                    SetAppSettings("JetMSGName", value);
                    
                }
                catch { }  
            }
        }

        private static string _JetFormat = "";
        public static string JetFormat
        {
            get
            {
                if (_JetFormat == "")
                {
                    try
                    {
                        _JetFormat = GetAppSettings("JetFormat");
                    }
                    catch { }
                }
                //_JetMSGName = (ConfigRow == null || ConfigRow["JetMSGName"] == DBNull.Value) ? "" : ConfigRow["JetMSGName"].ToString();
                return _JetFormat;
            }
            set
            {

                _JetFormat = value.ToString();
                try
                {
                    SetAppSettings("JetFormat", value);                    
                }
                catch { }
            }
        }
        
        private static bool IsFirstLoadSocketParaHeadInkJet = true;
        private static DataType.SocketParameter _SocketParaHeadInkJet;
        public static DataType.SocketParameter SocketParaHeadInkJet 
        {
            get
            {
                if (IsFirstLoadSocketParaHeadInkJet)
                {
                    try
                    {

                        _SocketParaHeadInkJet = new DataType.SocketParameter(GetAppSettings("SocketParaHeadInkJet"));
                    }
                    catch { }

                    IsFirstLoadSocketParaHeadInkJet = false;
                }

                return _SocketParaHeadInkJet;

            }

            set
            {
                _SocketParaHeadInkJet = value;
                try
                {
                    SetAppSettings("SocketParaHeadInkJet", _SocketParaHeadInkJet.GetParameterString());
                    
                }
                catch { }
            }
        }

        private static bool IsFirstLoadSocketParaTailInkJet = true;
        private static DataType.SocketParameter _SocketParaTailInkJet;
        public static DataType.SocketParameter SocketParaTailInkJet
        {
            get
            {
                if (IsFirstLoadSocketParaTailInkJet)
                {
                    try
                    {

                        _SocketParaTailInkJet = new DataType.SocketParameter(GetAppSettings("SocketParaTailInkJet"));
                    }
                    catch { }

                    IsFirstLoadSocketParaTailInkJet = false;
                }

                return _SocketParaTailInkJet;

            }

            set
            {
                _SocketParaTailInkJet = value;
                try
                {
                    SetAppSettings("SocketParaTailInkJet", _SocketParaTailInkJet.GetParameterString());
                    
                }
                catch { }
            }
        }                


        //PLC         
        private static bool IsFirstLoadPLCAddress = true;
        private static PLC.PLCAddress _PLCAddress;
        public static PLC.PLCAddress PLCAddress
        {
            get
            {
                if (IsFirstLoadPLCAddress)
                {
                    try
                    {
                        _PLCAddress = new PLC.PLCAddress(GetAppSettings("PLCAddress"));
                    }
                    catch { }

                    IsFirstLoadPLCAddress = false;
                }
                return _PLCAddress;
            }

            set
            {
                _PLCAddress = value;              
                try
                {
                    SetAppSettings("PLCAddress", _PLCAddress.GetParameterString());
                    
                }
                catch { }
            }
        }

        private static bool IsFirstLoadPLCAddress1 = true;
        private static PLC.PLCAddress _PLCAddress1;
        public static PLC.PLCAddress PLCAddress1
        {
            get
            {
                if (IsFirstLoadPLCAddress1)
                {
                    try
                    {
                        _PLCAddress1 = new PLC.PLCAddress(GetAppSettings("PLCAddress1"));
                    }
                    catch { }

                    IsFirstLoadPLCAddress1 = false;
                }
                return _PLCAddress1;
            }

            set
            {
                _PLCAddress1 = value;
                try
                {
                    SetAppSettings("PLCAddress1", _PLCAddress1.GetParameterString());
                    
                }
                catch { }
            }
        }

        private static bool IsFirstLoadPLCAddress2 = true;
        private static PLC.PLCAddress _PLCAddress2;
        public static PLC.PLCAddress PLCAddress2
        {
            get
            {
                if (IsFirstLoadPLCAddress2)
                {
                    try
                    {
                        _PLCAddress2 = new PLC.PLCAddress(GetAppSettings("PLCAddress2"));
                    }
                    catch { }

                    IsFirstLoadPLCAddress2 = false;
                }
                return _PLCAddress2;
            }

            set
            {
                _PLCAddress2 = value;
                try
                {
                    SetAppSettings("PLCAddress2", _PLCAddress2.GetParameterString());
                    
                }
                catch { }
            }
        }

        private static bool IsFirstLoadPLCAddress3 = true;
        private static PLC.PLCAddress _PLCAddress3;
        public static PLC.PLCAddress PLCAddress3
        {
            get
            {
                if (IsFirstLoadPLCAddress3)
                {
                    try
                    {
                        _PLCAddress3 = new PLC.PLCAddress(GetAppSettings("PLCAddress3"));
                    }
                    catch { }

                    IsFirstLoadPLCAddress3 = false;
                }
                return _PLCAddress3;
            }

            set
            {
                _PLCAddress3 = value;
                try
                {
                    SetAppSettings("PLCAddress3", _PLCAddress3.GetParameterString());
                    
                }
                catch { }
            }
        }

        private static bool IsFirstLoadPLCAddress4 = true;
        private static PLC.PLCAddress _PLCAddress4;
        public static PLC.PLCAddress PLCAddress4
        {
            get
            {
                if (IsFirstLoadPLCAddress4)
                {
                    try
                    {
                        _PLCAddress4 = new PLC.PLCAddress(GetAppSettings("PLCAddress4"));
                    }
                    catch { }

                    IsFirstLoadPLCAddress4 = false;
                }
                return _PLCAddress4;
            }

            set
            {
                _PLCAddress4 = value;
                try
                {
                    SetAppSettings("PLCAddress4", _PLCAddress4.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadPLCAddress5 = true;
        private static PLC.PLCAddress _PLCAddress5;
        public static PLC.PLCAddress PLCAddress5
        {
            get
            {
                if (IsFirstLoadPLCAddress5)
                {
                    try
                    {
                        _PLCAddress5 = new PLC.PLCAddress(GetAppSettings("PLCAddress5"));
                    }
                    catch { }

                    IsFirstLoadPLCAddress5 = false;
                }
                return _PLCAddress5;
            }

            set
            {
                _PLCAddress5 = value;
                try
                {
                    SetAppSettings("PLCAddress5", _PLCAddress5.GetParameterString());

                }
                catch { }
            }
        }

        private static bool IsFirstLoadPLCReadServerParameter = true;
        private static PLC.PLCReadServerParameter[] _PLCReadServerParameter;
        public static PLC.PLCReadServerParameter[] PLCReadServerParameter
        {
            get
            {
                if (IsFirstLoadPLCReadServerParameter)
                {
                    try
                    {             
                         string[] strpara=   GetAppSettings("PLCReadServerParameter").Split('|');
                         _PLCReadServerParameter = new PLC.PLCReadServerParameter[strpara.Length];
                        for(char c=(char)0;c<strpara.Length;c++)
                        {
                            _PLCReadServerParameter[c] = new PLC.PLCReadServerParameter(strpara[c]);
                        }
                    }
                    catch { }
                    IsFirstLoadPLCReadServerParameter = false;
                }
                return _PLCReadServerParameter;
            }

            set
            {
                _PLCReadServerParameter = value;
                try
                {
                    string[] strs = new string[_PLCReadServerParameter.Length];
                    for (char c = (char)0; c < _PLCReadServerParameter.Length; c++)
                    {
                        strs[c] = _PLCReadServerParameter[c].GetParameterString();
                    }
                    SetAppSettings("PLCReadServerParameter", String.Join("|", strs));
                }
                catch { }
            }
        }

        private static int _PLCReadCount = -1;
        public static int PLCReadCount
        {
            get
            {
                if (_PLCReadCount == -1)
                {
                    try
                    {
                        _PLCReadCount = Convert.ToInt32(GetAppSettings("PLCReadCount"));
                    }
                    catch
                    {
                        _PLCReadCount = 0;
                    }
                }
                //_PLCReadCount = Convert.ToInt32((ConfigRow == null || ConfigRow["PLCReadCount"] == DBNull.Value) ? "" : ConfigRow["PLCReadCount"].ToString());
                return _PLCReadCount;
            }

            set
            {

                _PLCReadCount = value;
                try
                {
                    SetAppSettings("PLCReadCount", value.ToString());
                    
                }
                catch { }
            }
        }

        private static int _PLCReadBegin = -1;
        public static int PLCReadBegin
        {
            get
            {
                if (_PLCReadBegin == -1)
                {
                    try
                    {
                        _PLCReadBegin = Convert.ToInt32(GetAppSettings("PLCReadBegin"));
                    }
                    catch { }

                }
                //_PLCReadBegin = Convert.ToInt32((ConfigRow == null || ConfigRow["PLCReadBegin"] == DBNull.Value) ? "" : ConfigRow["PLCReadBegin"].ToString());
                return _PLCReadBegin;
            }
            set
            {

                _PLCReadBegin = value;
                try
                {
                    SetAppSettings("PLCReadBegin", value.ToString());
                    
                }
                catch { }
            }
        }

        private static int _PLCDBNO = -1;
        public static ushort PLCDBNO
        {
            get
            {
                if (_PLCDBNO == -1)
                {
                    try
                    {
                        _PLCDBNO = Convert.ToInt32(GetAppSettings("PLCDBNO"));
                    }
                    catch { }

                }
                //_PLCDBNO = Convert.ToInt32((ConfigRow == null || ConfigRow["PLCDBNO"] == DBNull.Value) ? "" : ConfigRow["PLCDBNO"].ToString());
                return (ushort)_PLCDBNO;
            }

            set
            {

                _PLCDBNO = value;
                try
                {
                    SetAppSettings("PLCDBNO", value.ToString());
                    
                }
                catch { }
            }
        }
        
        public static short PLCDefaultDS_ID = 999;

        public static string LocalIPAddress = "";
        public static Assembly AppAssembly;

        private static string _StyleName = "";
        public static string StyleName
        {
            get
            {
                if (_StyleName == "")
                {
                    try
                    {
                        _StyleName = GetAppSettings("StyleName");
                    }
                    catch { }
                }
                return _StyleName;
            }

            set
            {
                _StyleName = value;
                try
                {
                    SetAppSettings("StyleName", value);
                    
                }
                catch { }              
            }
        }

        private static bool _IsVacuum = false;  //是否压缩日志文件
        public static bool IsVacuum
        {
            get
            {
                try
                {
                    _IsVacuum = Convert.ToBoolean( GetAppSettings("IsVacuum"));
                }
                catch { }
                return _IsVacuum;
            }

            set
            {
                _IsVacuum = value;
                try
                {
                    SetAppSettings("IsVacuum", value.ToString());                    
                    
                }
                catch { }
            }
        }

        private static string _AlarmLogPath = "";
        public static string AlarmLogPath
        {
            get
            {
                if (_AlarmLogPath == "")
                {

                    try
                    {
                        _AlarmLogPath = GetAppSettings("AlarmLogPath");
                    }
                    catch { }
                }
                return _AlarmLogPath;
            }

            set
            {
                _AlarmLogPath = value;
                try
                {
                    SetAppSettings("AlarmLogPath", value);           
                }
                catch { }
            }
        }

        private static string _Position = "";
        public static string Position
        {
            get
            {
                if (_Position == "")
                {
                    try
                    {
                        _Position = GetAppSettings("Position");
                    }
                    catch { }
                }
                    //_Position = (ConfigRow == null || ConfigRow["Position"] == DBNull.Value) ? "" : ConfigRow["Position"].ToString();
                return _Position;
            }

            set
            {
                 
                        _Position = value;
                        try
                        {
                            SetAppSettings("Position", value);
                            
                        }
                        catch { }  
            }
        }

        private static string _Scan1Position = "";
        public static string Scan1Position
        {
            get
            {
                if (_Scan1Position == "")
                {
                    try
                    {
                        _Scan1Position = GetAppSettings("Scan1Position");
                    }
                    catch { }
                }
                   // _Scan1Position = (ConfigRow == null || ConfigRow["Scan1Position"] == DBNull.Value) ? "" : ConfigRow["Scan1Position"].ToString();
                return _Scan1Position;
            }

            set
            {
                 
                _Scan1Position = value;
                try
                {
                    SetAppSettings("Scan1Position", value);
                    
                }
                catch { }  
                
            }
        }

        private static string _Scan2Position = "";
        public static string Scan2Position
        {
            get
            {
                if (_Scan2Position == "")
                {
                    try
                    {
                        _Scan2Position = GetAppSettings("Scan2Position");
                    }
                    catch { }
                }
                    //_Scan2Position = (ConfigRow == null || ConfigRow["Scan2Position"] == DBNull.Value) ? "" : ConfigRow["Scan2Position"].ToString();
                return _Scan2Position;
            }

            set
            {
               
                        _Scan2Position = value;
                        try
                        {
                            SetAppSettings("Scan2Position", value);
                            
                        }
                        catch { }  
            }
        }

        private static string _Scan3Position = "";
        public static string Scan3Position
        {
            get
            {
                if (_Scan3Position == "")
                {
                    try
                    {
                        _Scan3Position = GetAppSettings("Scan3Position");
                    }
                    catch { }
                }
                    //_Scan3Position = (ConfigRow == null || ConfigRow["Scan3Position"] == DBNull.Value) ? "" : ConfigRow["Scan3Position"].ToString();
                return _Scan3Position;
            }

            set
            {
                
                        _Scan3Position = value;
                        try
                        {
                            SetAppSettings("Scan3Position", value);
                            
                        }
                        catch { }  
            }
        }

        private static string  _IsTestMode = ""; //"0" ,"1"
        public static bool IsTestMode
        {

            get
            {
                if (_IsTestMode == "")
                {
                    try
                    {
                        _IsTestMode =   GetAppSettings("IsTestMode");
                    }
                    catch { }
                }
                    //_IsTestMode = (ConfigRow == null || ConfigRow["TestMode"] == DBNull.Value) ? "": ConfigRow["TestMode"].ToString();
            
                return _IsTestMode.ToLower().Equals("true"); //默认除了"true"以外其他都为false
            }

            set
            {
                 
                        _IsTestMode = value.ToString();
                        try
                        {
                            SetAppSettings("IsTestMode", value.ToString());
                            
                        }
                        catch { }  
            }
        }

        private static string _IsDebugMode = ""; //"0" ,"1"
        public static bool IsDebugMode
        {
            get
            {
                if (_IsDebugMode == "")
                {
                    try
                    {
                        _IsDebugMode = GetAppSettings("IsDebugMode");
                    }
                    catch { }
                }
                return _IsDebugMode.ToLower().Equals("true"); //默认除了"true"以外其他都为false
            }
            set
            {
                _IsDebugMode = value.ToString();
                try
                {
                    SetAppSettings("IsDebugMode", value.ToString());
                }
                catch { }
            }
        }

        private static int _WidthMax= -1;
        public static int WidthMax 
        {
            get
            {
                if (_WidthMax == -1)
                {
                    try
                    {
                        _WidthMax = Convert.ToInt32( GetAppSettings("WidthMax"));
                    }
                    catch { }
                }
                    //_WidthMax = Convert.ToInt32((ConfigRow == null || ConfigRow["WidthMax"] == DBNull.Value) ? "" : ConfigRow["WidthMax"].ToString());
                return _WidthMax;
            }

            set
            {
                
                        _WidthMax = value;
                        try
                        {
                            SetAppSettings("WidthMax", value.ToString());
                            
                        }
                        catch { }  
            }
        }

        private static string _MachineCode = "";
        public static string MachineCode
        {
            get
            {
                if (_MachineCode == "")
                {
                    try
                    {
                        _MachineCode = GetAppSettings("MachineCode");
                    }
                    catch { }
                }
                return _MachineCode;
            }

            set
            {
                _MachineCode = value;
                try
                {
                    SetAppSettings("MachineCode", value);
                    
                }
                catch { }
            }
        }

        private static string _Machine = "";
        public static string Machine
        {
            get
            {
                if (_Machine == "")
                {
                    try
                    {
                        _Machine = GetAppSettings("Machine");
                    }
                    catch { }
                }
                return _Machine;
            }

            set
            {
                _Machine = value;
                try
                {
                    SetAppSettings("Machine", value);
                    
                }
                catch { }
            }
        }

        private static string _Company = "";
        public static string Company
        {
            get
            {
                if (_Company == "")
                {
                    try
                    {
                        _Company = GetAppSettings("Company");
                    }
                    catch { }
                }
                return _Company;
            }

            set
            {
                _Company = value;
                try
                {
                    SetAppSettings("Company", value);                    
                }
                catch { }
            }
        }

        private static string _Factory = "";
        public static string Factory
        {
            get
            {
                if (_Factory == "")
                {
                    try
                    {
                        _Factory = GetAppSettings("Factory");
                    }
                    catch { }
                }
                return _Factory;
            }

            set
            {
                _Factory = value;
                try
                {
                    SetAppSettings("Factory", value);
                    
                }
                catch { }
            }
        }

        private static string _ProducePart = "";
        public static string ProducePart
        {
            get
            {
                if (_ProducePart == "")
                {
                    try
                    {
                        _ProducePart = GetAppSettings("ProducePart");
                    }
                    catch { }
                }
                return _ProducePart;
            }

            set
            {
                _ProducePart = value;
                try
                {
                    SetAppSettings("ProducePart", value);
                    
                }
                catch { }
            }
        }

        private static Int64 _VersionID = 0;
        public static Int64 VersionID
        {
            get
            {
                if (_VersionID == 0)
                    try
                    {

                        _VersionID = Convert.ToInt32(GetAppSettings("VersionID"));

                    }
                    catch { }
                return _VersionID;
            }

            set
            {

                _VersionID = value;
                try
                {
                    SetAppSettings("VersionID", value.ToString());
                    
                }
                catch { }
            }
        }

      

        private static string _WidthMeasureParameter = "";
        public static string WidthMeasureParameter
        {
            get
            {
                if (_WidthMeasureParameter == "")
                {
                    try
                    {
                        _WidthMeasureParameter =  GetAppSettings("WidthMeasureParameter").Trim();
                    }
                    catch { }
                }
                
                return _WidthMeasureParameter;
            }

            set
            {

                _WidthMeasureParameter = value;
                try
                {
                    SetAppSettings("WidthMeasureParameter", value.ToString());
                    
                }
                catch { }
            }
        }
        private static string _WidthMeasureParameter2 = "";
        public static string WidthMeasureParameter2
        {
            get
            {
                if (_WidthMeasureParameter2 == "")
                {
                    try
                    {
                        _WidthMeasureParameter2 = GetAppSettings("WidthMeasureParameter2").Trim();
                    }
                    catch { }
                }

                return _WidthMeasureParameter2;
            }

            set
            {

                _WidthMeasureParameter2 = value;
                try
                {
                    SetAppSettings("WidthMeasureParameter2", value.ToString());

                }
                catch { }
            }
        }
       
        private static string _DiameterMeasureParameter = "";
        public static string DiameterMeasureParameter
        {
            get
            {
                if (_DiameterMeasureParameter == "")
                {
                    try
                    {
                        _DiameterMeasureParameter = GetAppSettings("DiameterMeasureParameter").Trim();
                    }
                    catch { }
                }

                return _DiameterMeasureParameter;
            }

            set
            {

                _DiameterMeasureParameter = value;
                try
                {
                    SetAppSettings("DiameterMeasureParameter", value.ToString());
                    
                }
                catch { }
            }
        }

        private static string _WeightIgnoreModeIsPercent = "";
        public static bool WeightIgnoreModeIsPercent
        {
            get
            {
                if (_WeightIgnoreModeIsPercent == "")
                {
                    try
                    {
                        _WeightIgnoreModeIsPercent = GetAppSettings("WeightIgnoreModeIsPercent");//(ConfigRow == null || ConfigRow["WeightIgnoreMode"] == DBNull.Value) ? "0" : ConfigRow["WeightIgnoreMode"].ToString();
                    }
                    catch { }
                }
                return !_WeightIgnoreModeIsPercent.ToLower().Equals("false"); //默认除了"false"以外其他都为Percent
            }
            set
            {
                _WeightIgnoreModeIsPercent = value.ToString();
                        try
                        {
                            SetAppSettings("WeightIgnoreModeIsPercent", value.ToString());
                            
                        }
                        catch { }  
            }
        }
        private static string _Stereo = "";
        public static bool Stereo
        {
            get
            {
                if (_Stereo == "")
                {
                    try
                    {
                        _Stereo = GetAppSettings("Stereo");
                    }
                    catch { }
                }
                return !_Stereo.ToLower().Equals("false");
            }
            set
            {
                _Stereo = value.ToString();
                try
                {
                    SetAppSettings("Stereo", value.ToString());

                }
                catch { }
            }
        }
        private static DateTime _AppLogTimeStart = DateTime.Now;//记录起始时间
        private static bool IsDateTimeNowFirstLoad = true;
        private static Stopwatch sw = new Stopwatch();
        public static DateTime DateTimeNow 
        {
            get
            {
                if (IsDateTimeNowFirstLoad)
                {
                    IsDateTimeNowFirstLoad = false;
                    Utils.DateTimeNow = DateTime.Now;                
                }

                return _AppLogTimeStart.Add(sw.Elapsed);
                 
                
            }

            set
            {
                if (!sw.IsRunning)
                {
                    sw.Reset();
                    sw.Start();
                }
                _AppLogTimeStart = value;
               
              

            }
        
        }                   
       
        //PLCSocketServer
        private static string _PLCReadMode =  "0";
        public static string PLCReadMode
        {
            get
            {
                if (_PLCReadMode == "0")
                {
                    try
                    {
                        PLCReadMode = GetAppSettings("PLCReadMode");
                    }
                    catch { }
                }
                //_PLCReadMode = (ConfigRow == null || ConfigRow["PLCReadMode"] == DBNull.Value) ? "" : ConfigRow["PLCReadMode"].ToString();
                return _PLCReadMode;
            }

            set
            {
               
                        _PLCReadMode = value;
                        try
                        {
                            SetAppSettings("PLCReadMode", value.ToString());
                            
                        }
                        catch { }  
            }
        }

        private static bool IsFirstLoadSocketParaPLCReadServer = true;
        private static DataType.SocketParameter _SocketParaPLCReadServer;
        public static DataType.SocketParameter SocketParaPLCReadServer
        {
            get
            {
                if (IsFirstLoadSocketParaPLCReadServer)
                {
                    try
                    {

                        _SocketParaPLCReadServer = new DataType.SocketParameter(GetAppSettings("SocketParaPLCReadServer"));
                    }
                    catch { }

                    IsFirstLoadSocketParaPLCReadServer = false;
                }

                return _SocketParaPLCReadServer;

            }

            set
            {
                _SocketParaPLCReadServer = value;
                try
                {
                    SetAppSettings("SocketParaPLCReadServer", _SocketParaPLCReadServer.GetParameterString());
                    
                }
                catch { }
            }
        }
        
        public static char SocketDelimiterPLCCommunication = '>';
        private static bool IsFirstLoadSocketParaPLCCommunication = true;
        private static DataType.SocketParameter _SocketParaPLCCommunication;
        public static DataType.SocketParameter SocketParaPLCCommunication
        {
            get
            {
                if (IsFirstLoadSocketParaPLCCommunication)
                {
                    try
                    {
                        _SocketParaPLCCommunication = new DataType.SocketParameter(GetAppSettings("SocketParaPLCCommunication"));
                    }
                    catch { }
                    IsFirstLoadSocketParaPLCCommunication = false;
                }
                return _SocketParaPLCCommunication;
            }

            set
            {
                _SocketParaPLCCommunication = value;
                try
                {
                    SetAppSettings("SocketParaPLCCommunication", _SocketParaPLCCommunication.GetParameterString());
                }
                catch { }
            }
        }


        private static bool IsFirstLoadSocketParaChaintServer = true;
        private static DataType.SocketParameter _SocketParaChaintServer;
        public static DataType.SocketParameter SocketParaChaintServer
        {
            get
            {
                if (IsFirstLoadSocketParaChaintServer)
                {
                    try
                    {

                        _SocketParaChaintServer = new DataType.SocketParameter(GetAppSettings("SocketParaChaintServer"));
                    }
                    catch { }

                    IsFirstLoadSocketParaChaintServer = false;
                }

                return _SocketParaChaintServer;

            }

            set
            {
                _SocketParaChaintServer = value;
                try
                {
                    SetAppSettings("SocketParaChaintServer", _SocketParaChaintServer.GetParameterString());
                    
                }
                catch { }
            }
        }       

        //RemoteUpdate
        private static string _RemoteUpdatePath = "";
        public static string RemoteUpdatePath
        {
            get
            {
                if (_RemoteUpdatePath == "")
                {
                    try
                    {
                        _RemoteUpdatePath = GetAppSettings("RemoteUpdatePath");
                    }
                    catch { }

                }
                //_RemoteUpdatePath = (ConfigRow == null || ConfigRow["RemoteUpdatePath"] == DBNull.Value) ? "" : ConfigRow["RemoteUpdatePath"].ToString();
                return _RemoteUpdatePath;
            }

            set
            {
                 
                        _RemoteUpdatePath = value;
                        try
                        {
                            SetAppSettings("RemoteUpdatePath", value.ToString());
                            
                        }
                        catch { }  
            }
        }


        private static string _ApplicationName = "";
        public static string ApplicationName
        {
            get
            {
                if (_ApplicationName == "")
                {
                    try
                    {
                        _ApplicationName = GetAppSettings("ApplicationName");
                    }
                    catch { }
                }
                return _ApplicationName;
            }
            set
            {
                _ApplicationName = value.ToString();
                try
                {
                    SetAppSettings("ApplicationName", value);
                }
                catch { }
            }
        }

        //public static MainDS.Paper_DSDataTable Paper_DSTB = new MainDS.Paper_DSDataTable();
        public static MainDS.Paper_DestinationDataTable _Paper_DestinationTB;
        public static MainDS.Paper_DestinationDataTable Paper_DestinationTB
        {
            get
            {
                if (_Paper_DestinationTB == null)
                {
                    try
                    {
                        _Paper_DestinationTB = Utils.MSSqlAccess.Paper_DestinationQueryAll().Paper_Destination;  //GetAppSettings("RemoteUpdatePath");
                    }
                    catch { }

                }
                //_RemoteUpdatePath = (ConfigRow == null || ConfigRow["RemoteUpdatePath"] == DBNull.Value) ? "" : ConfigRow["RemoteUpdatePath"].ToString();
                return _Paper_DestinationTB;
            }
        }
       // public static MainDS.KONE_DSDataTable KONE_DSTB = new MainDS.KONE_DSDataTable(); 

        private static MainDS _PageInitDS;
        public static MainDS PageInitDS
        {
            get
            {
                if (_PageInitDS == null)
                {
                    try
                    {
                        _PageInitDS = Utils.MSSqlAccess.PageInitDSQueryAll();
                    }
                    catch { }

                }
                return _PageInitDS;
            }
        }
        
        private static ERPDS _LoadERPDS;
        public static ERPDS LoadERPDS
        {
            get
            {
                if (_LoadERPDS == null)
                {
                    try
                    {
                        _LoadERPDS = Utils.MSSqlAccess.ERPDSInitSQueryAll();
                    }
                    catch { }

                }
                return _LoadERPDS;
            }
        }
        
        private static string _PageDatas = "";
        public static string PageDatas
        {
            get
            {
                if (_PageDatas == "")
                {
                    try
                    {
                        _PageDatas = GetAppSettings("PageDatas").Trim();
                    }
                    catch { }
                }
                return _PageDatas;
            }
            set
            {
                _PageDatas = value;
                try
                {
                    SetAppSettings("PageDatas", value.ToString());
                }
                catch { }
            }
        }

        private static int _MaxInfoCount = -1;
        public static int MaxInfoCount
        {
            get
            {
                if (_MaxInfoCount == -1)
                {
                    try
                    {
                        _MaxInfoCount = Convert.ToInt32(GetAppSettings("MaxInfoCount"));
                    }
                    catch { }

                }
                    //_MaxInfoCount = Convert.ToInt32((ConfigRow == null || ConfigRow["MaxInfoCount"] == DBNull.Value) ? "" : ConfigRow["MaxInfoCount"].ToString());
                return _MaxInfoCount;
            }

            set
            {
               
                        _MaxInfoCount = value;
                        try
                        {
                            SetAppSettings("MaxInfoCount", value.ToString());
                            
                        }
                        catch { }  
            }
        }
        
        public static void SetAppSettings(string Entry, string Value)
        {
            MyAppConfig.SetValue("appSettings", Entry, Value);
        }
        public static void SetAppSettings(string Section, string Entry, string Value)
        {
            MyAppConfig.SetValue(Section, Entry, Value);
        }

        public static string GetAppSettings(string key)
        {
            return GetConfigValue("appSettings", key);
        }
        public static string GetconnectionStrings(string key)
        {
            return GetConfigValue("connectionStrings", key);
        }
       


        public static string GetConfigValue(string Section, string Entry)
        {
            //测试,第一次读取到的数据重新备份一下 
            string Value =  Convert.ToString(MyAppConfig.GetValue(Section, Entry)).Trim();
            MyAppConfigBak.SetValue(Section, Entry, Value);
            return Value;
        }







        //资源
        private static Image _ChaintLogo = null;
        public static Image ChaintLogo
        {
            get
            {
                if (_ChaintLogo == null)
                    _ChaintLogo = Image.FromFile(Utils.FilePath_ImagesDir + "ChaintLogo.png");
                return _ChaintLogo;
            }
        }

        private static ImageList _MessageImagelist = null;
        public static ImageList MessageImagelist
        {
            get
            {
                if (_MessageImagelist == null)
                {
                    _MessageImagelist = new ImageList();
                    _MessageImagelist.Images.Add(Image.FromFile(Utils.FilePath_ImagesDir + "OK.png"));
                    _MessageImagelist.Images.Add(Image.FromFile(Utils.FilePath_ImagesDir + "Warning.png"));
                    _MessageImagelist.Images.Add(Image.FromFile(Utils.FilePath_ImagesDir + "Error.png"));
                }
                return _MessageImagelist;
            }
        }
        /// <summary>
        /// 仓库的各种消息
        /// </summary>
        public class WMSMessage
        {
            /// <summary>
            /// 组合消息
            /// </summary>
            /// <param name="msgs">要发送的各个字段</param>
            /// <returns>各字段加上分隔符</returns>
            public static string MakeWMSSocketMsg(string[] msgs)
            {
                string retMsg = _SpliteChar + "";
                retMsg += String.Join(_SpliteChar + "", msgs);
                retMsg.TrimEnd(_SpliteChar);
                return _StartChar + (retMsg.Length + 6).ToString().PadLeft(4, '0') + retMsg + _EndChar;
            }
            //public static char _StartChar = (char)2, _EndChar = (char)3, _SpliteChar = (char)4, _ColumnChar = (char)5, _ForeachChar = (char)6;
            public static char _StartChar = (char)2, _EndChar = (char)3, _SpliteChar = (char)4, _ForeachChar = (char)5,_ColumnChar=(char)6;
            /// <summary>
            /// 去掉小数后面的0和小数点  123.000 = 123  ； 123.120 = 123.12；123.123=123.123
            /// </summary>
            /// <param name="number">数字型</param>
            /// <returns></returns>
            public static string TrimEndZero(string number){
                if (number.Contains(".")) {
                    number = number.TrimEnd('0').TrimEnd('.');
                }
                return number;
            }
        }
        /// <summary>
        /// 仓库的各种操作及状态
        /// </summary>
       public class WMSOperate
       {
           /// <summary>
           /// 扫描入库
           /// </summary>
           public const string _OperScanIn = "扫描入库";
           /// <summary>
           /// 取消扫描入库
           /// </summary>
           public const string _OperScanInCancel = "取消扫描入库";
           /// <summary>
           /// 红单扫描入库
           /// </summary>
           public const string _OperScanRedIn = "红单扫描入库";
           /// <summary>
           /// 取消红单扫描入库
           /// </summary>
           public const string _OperScanRedInCancel = "取消红单扫描入库";
           /// <summary>
           /// 扫描出库
           /// </summary>
           public const string _OperScanOut = "扫描出库";
           /// <summary>
           /// 取消扫描出库
           /// </summary>
           public const string _OperScanOutCancel = "取消扫描出库";
           /// <summary>
           /// 扫描红单出库
           /// </summary>
           public const string _OperScanOutRed = "红单扫描出库";
           /// <summary>
           /// 取消红单扫描出库
           /// </summary>
           public const string _OperScanOutRedCancel = "取消红单扫描出库";
           /// <summary>
           /// 执行出库
           /// </summary>
           public const string _OperExecOut = "执行出库";
           /// <summary>
           /// 取消执行出库
           /// </summary>
           public const string _OperExecOutCancel = "取消执行出库";
           /// <summary>
           /// in
           /// </summary>
           public  const string _StatusIn = "in";
           /// <summary>
           /// cancelin
           /// </summary>
           public  const string _StatusCancelIn = "cancelin";
           /// <summary>
           /// redin
           /// </summary>
           public  const string _StatusRedIn = "redin";
           /// <summary>
           /// cancelredin
           /// </summary>
           public  const string _StatusCancelRedIn = "cancelredin";
           /// <summary>
           /// out
           /// </summary>
           public  const string _StatusOut = "out";
           /// <summary>
           ///cancel out
           /// </summary>
           public  const string _StatusCancelOut = "cancelout";
           /// <summary>
           ///red out
           /// </summary>
           public  const string _StatusRedOut = "redout";
           /// <summary>
           ///cancelred out
           /// </summary>
           public  const string _StatusCancelRedOut = "cancelredout";
          

       }
        /// <summary>
        /// 系统主菜单点击操作
        /// </summary>
       public class WMSMenu {
           public  const string _New = "new";
           public  const string _Edit = "edit";
           public  const string _Delete = "delete";
           public  const string _Preview = "preview";
           public  const string _Next = "next";
           public  const string _Check = "check";
           public  const string _Uncheck = "uncheck";
           public const string _Save = "save";
           public const string _ReFresh = "refresh";
           public const string _NewLine = "newline";
           public const string _DelLine = "delline";
           public const string _Exit = "exit";
           public const string _View = "view";

           public const string _Lock = "lock";
       
       }

       /// <summary>
       /// 单据类型
       /// </summary>
       public class WMSVoucherType
       {
           public const string _BillIn = "CTI";
           public const string _BillPlan = "CTP";
           public const string _BillOut = "CTO";
           //public const string _Preview = "preview";
           //public const string _Next = "next";
           //public const string _Check = "check";
           //public const string _Uncheck = "uncheck";
           //public const string _Save = "save";
           //public const string _ReFresh = "refresh";


       }
    } 
}
