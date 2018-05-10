using System.Data.SqlClient;
using Chaint.Common.Core.Enums;

namespace Chaint.Common.Data
{
    public class SqlUtils
    {
        public static string GetConnectionString(DBConnectParams dbParams)
        {
            string strConnectionString = "";
            if (dbParams == null) return strConnectionString;
            switch (dbParams.DBType)
            {
                case Enums_DBType.MSSQL:
                    strConnectionString=  GetMSSqlConnetionString(dbParams);
                    break;
                default:
                    strConnectionString = GetMSSqlConnetionString(dbParams);
                    break;
            }
            return strConnectionString;
        }
        private static string GetMSSqlConnetionString(DBConnectParams dbParams)
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = dbParams.Server;
            sb.InitialCatalog = dbParams.DataBase;
            sb.UserID = dbParams.UserID;
            sb.Password = dbParams.Password;
            return sb.ToString();
        }
    }
}
