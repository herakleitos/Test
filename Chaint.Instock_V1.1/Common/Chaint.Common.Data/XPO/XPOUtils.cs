using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Chaint.Common.Core.Enums;
namespace Chaint.Common.Data.XPO
{
 
    public class XPOUtils
    {
        public static string GetConnectionString(DBConnectParams dbParams)
        {
            if (dbParams == null) return XpoDefault.GetDataLayer(AutoCreateOption.DatabaseAndSchema).Connection.ConnectionString;
            string strConnectionString = "";
            switch (dbParams.DBType)
            {
                case Enums_DBType.ACCESS:
                    if (dbParams.UserID.Trim().Length == 0 && dbParams.Password.Trim().Length == 0)
                        strConnectionString = AccessConnectionProvider.GetConnectionString(dbParams.DataBase);
                    else
                        strConnectionString = 
                            AccessConnectionProvider.GetConnectionString(dbParams.DataBase, dbParams.UserID, dbParams.Password);
                    break;
                case Enums_DBType.SQLITE:
                    strConnectionString = SQLiteConnectionProvider.GetConnectionString(dbParams.DataBase);
                    break;
                case Enums_DBType.MSSQL:
                    strConnectionString = MSSqlConnectionProvider.GetConnectionString(dbParams.Server,dbParams.UserID, dbParams.Password, dbParams.DataBase);
                    break;

                case Enums_DBType.MYSQL:
                    strConnectionString = MySqlConnectionProvider.GetConnectionString(dbParams.Server, dbParams.UserID, dbParams.Password, dbParams.DataBase);
                    break;

                case Enums_DBType.SYBASE:
                    strConnectionString = AsaConnectionProvider.GetConnectionString(dbParams.Server, dbParams.DataBase, dbParams.UserID, dbParams.Password);
                    break;

                case Enums_DBType.DB2:
                    strConnectionString = DB2ConnectionProvider.GetConnectionString(dbParams.Server, dbParams.DataBase, dbParams.UserID, dbParams.Password);
                    break;

                case Enums_DBType.ORACLE:
                    strConnectionString = OracleConnectionProvider.GetConnectionString(dbParams.Server, dbParams.UserID, dbParams.Password);
                    break;
                //默认为MSSql 类型数据库
                default:
                    strConnectionString = MSSqlConnectionProvider.GetConnectionString(dbParams.Server, dbParams.UserID, dbParams.Password, dbParams.DataBase);
                    break;

            }
            return strConnectionString;
        }
        /// <summary>
        /// 通过数据层创建工作单元(实际上就是一个继承自Session的对象)
        /// </summary>
        /// <param name="dataLayer"></param>
        /// <returns></returns>
        public static UnitOfWork CreateUnitOfWorkByDataLayer(IDataLayer dataLayer)
        {
            UnitOfWork session = new UnitOfWork(dataLayer);

            return session;
        }
        /// <summary>
        /// 根据连接串，创建一个数据层接口
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static IDataLayer CreateDataLayer(string connectionString)
        {
            IDataLayer dataLayer = null;
            try
            {
                dataLayer = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return dataLayer;
        }
        /// <summary>
        /// 通过数据层创建工作单元(实际上就是一个继承自Session的对象)
        /// </summary>
        /// <param name="strConnectString"></param>
        /// <returns></returns>
        public static UnitOfWork CreateUnitOfWork(string strConnectString)
        {
            return new UnitOfWork(CreateDataLayer(strConnectString));
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="strConnectString"></param>
        /// <returns></returns>
        public static UnitOfWork CreateUnitOfWorkForCreateDB(string strConnectString)
        {
            IDataLayer dataLayer = 
                XpoDefault.GetDataLayer(strConnectString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            return new UnitOfWork(dataLayer);
        }
    }
}
