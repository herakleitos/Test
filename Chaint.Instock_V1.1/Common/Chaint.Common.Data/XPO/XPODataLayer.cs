using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
namespace Chaint.Common.Data.XPO
{
    public class XPODataLayer
    {
        #region 获取不同的数据层接口

        /// <summary>
        /// 获取Sqlite对应的数据层接口
        /// </summary>
        /// <param name="strDBFilePath"></param>
        /// <returns></returns>
        public static IDataLayer GetSqliteDataLayerByDatabasePath(string strDBFilePath)
        {
            string strConnectString = 
                SQLiteConnectionProvider.GetConnectionString(strDBFilePath);
            return CreateDataLayer(strConnectString);
        }
        /// <summary>
        /// SQL Server对应的数据层接口
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDatabase"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static IDataLayer GetMSSqlDataLayerByConnectString(string strServer, string strDatabase, string strUserID, string strPassword)
        {
            string strConnectString = 
                MSSqlConnectionProvider.GetConnectionString(strServer, strUserID, strPassword, strDatabase);
            return CreateDataLayer(strConnectString);
        }
        /// <summary>
        /// MySql对应的数据层接口
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDatabase"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static IDataLayer GetMySqlDataLayerByConnectString(string strServer, string strDatabase, string strUserID, string strPassword)
        {
            string strConnectString = 
                MySqlConnectionProvider.GetConnectionString(strServer, strUserID, strPassword, strDatabase);
            return CreateDataLayer(strConnectString);
        }
        /// <summary>
        /// Oracle对应的数据层接口
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static IDataLayer GetOracleDataLayerByConnectString(string strServer, string strUserID, string strPassword)
        {
            string strConnectString = 
                OracleConnectionProvider.GetConnectionString(strServer, strUserID, strPassword);
            return CreateDataLayer(strConnectString);
        }
        /// <summary>
        /// DB2对应的数据层接口
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDatabase"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static IDataLayer GetDB2DataLayerByConnectString(string strServer, string strDatabase, string strUserID, string strPassword)
        {
            string strConnectString = 
                DB2ConnectionProvider.GetConnectionString(strServer, strDatabase, strUserID, strPassword);
            return CreateDataLayer(strConnectString);
        }
        /// <summary>
        /// Sybase数据库
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDatabase"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static IDataLayer GetSybaseDataLayerByConnectString(string strServer, string strDatabase, string strUserID, string strPassword)
        {
            string strConnectString = 
                AsaConnectionProvider.GetConnectionString(strServer, strDatabase, strUserID, strPassword);
            return CreateDataLayer(strConnectString);
        }
        /// <summary>
        /// Access对应的数据层接口
        /// </summary>
        /// <param name="strDatabase"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static IDataLayer GetAccessDataLayerByConnectString(string strDatabase, string strUserID, string strPassword)
        {
            string strConnectString = 
                AccessConnectionProvider.GetConnectionString(strDatabase, strUserID, strPassword);
            return CreateDataLayer(strConnectString);
        }
        /// <summary>
        /// Access 数据层
        /// </summary>
        /// <param name="strDatabase">Access文件全路径</param>
        /// <returns></returns>
        public static IDataLayer GetAccessDataLayerByConnectString(string strDatabase)
        {
            string strConnectString = 
                AccessConnectionProvider.GetConnectionString(strDatabase);
            return CreateDataLayer(strConnectString);
        }
        #endregion
        private static IDataLayer CreateDataLayer(string connectionString)
        {
            IDataLayer dataLayer = null;
            try
            {
                dataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return dataLayer;
        }
    }
}
