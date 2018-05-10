using System.Data;
using System.Data.SqlClient;
using Chaint.Common.Data.XPO;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Sql.DataApi;
using Chaint.Common.Core.AppConfig;
using Chaint.Common.Core;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.Enums;
namespace Chaint.Common.Data
{
    public class DBService
    {
        private static Context _context = null;
        private static XPODBAccessor _xpoAccessor = null;
        private static SqlDataAccessor _sqlAccessor = null;
        private static XPODBAccessor MainAccessor
        {
            get
            {
                if (_xpoAccessor == null)
                {
                    AppConfig_INI AppConfiger = new AppConfig_INI(_context.AppConfigFilePath);
                    DBConnectParams dbParam = new DBConnectParams()
                    {
                        DBType = Enums_DBType.MSSQL,
                        Server = AppConfiger.GetValue(_context.Section, "Server", ""),
                        DataBase = AppConfiger.GetValue(_context.Section, "DataBase", ""),
                        UserID = AppConfiger.GetValue(_context.Section, "UserID", "sa"),
                        Password = AppConfiger.GetValue(_context.Section, "Password", "chaint")
                    };
                    string strConnectionString = XPOUtils.GetConnectionString(dbParam);
                    _xpoAccessor = new XPODBAccessor(dbParam.DBType, strConnectionString);
                }
                return _xpoAccessor;
            }
            set { _xpoAccessor = value; }
        }
        private static SqlDataAccessor SqlAccessor
        {
            get
            {
                if (_sqlAccessor == null)
                {
                    AppConfig_INI AppConfiger = new AppConfig_INI(_context.AppConfigFilePath);
                    DBConnectParams dbParam = new DBConnectParams()
                    {
                        DBType = Enums_DBType.MSSQL,
                        Server = AppConfiger.GetValue(_context.Section, "Server", ""),
                        DataBase = AppConfiger.GetValue(_context.Section, "DataBase", ""),
                        UserID = AppConfiger.GetValue(_context.Section, "UserID", "sa"),
                        Password = AppConfiger.GetValue(_context.Section, "Password", "chaint")
                    };
                    string strConnectionString = SqlUtils.GetConnectionString(dbParam);
                    _sqlAccessor = new SqlDataAccessor(dbParam.DBType, strConnectionString);
                }
                return _sqlAccessor;
            }
            set { _sqlAccessor = value; }
        }
        public static ITable ExecuteQuery(Context ctx, string strSQL, params QueryParameter[] Parameters)
        {
            if (_context == null)
            {
                _context = ctx.Clone();
            }
            else if (!_context.AppConfigFilePath.EqualIgnorCase(ctx.AppConfigFilePath) ||
                !_context.Section.EqualIgnorCase(ctx.Section))
            {
                //上下文改变，重新设置链接
                _context = ctx.Clone();
                ClearAccess();
            }
            return MainAccessor.ExecuteQuery(strSQL, Parameters);
        }
        public static DataTable ExecuteQueryWithDataTable(Context ctx, string strSQL, params SqlParameter[] Parameters)
        {
            if (_context == null)
            {
                _context = ctx.Clone();
            }
            else if (!_context.AppConfigFilePath.EqualIgnorCase(ctx.AppConfigFilePath) ||
                !_context.Section.EqualIgnorCase(ctx.Section))
            {
                //上下文改变，重新设置链接
                _context = ctx.Clone();
                ClearAccess();
            }
            string[] tableName = new string[] { "T1"};
            DataSet ds= SqlAccessor.ExecuteQuery(strSQL, tableName, Parameters);
            if (ds == null||ds.Tables.Count<=0) return null;
            return ds.Tables["T1"];
        }
        public static int ExecuteNonQuery(Context ctx, string strSQL, params QueryParameter[] parameters)
        {
            if (_context == null)
            {
                _context = ctx.Clone();
            }
            else if (!_context.AppConfigFilePath.EqualIgnorCase(ctx.AppConfigFilePath) ||
                !_context.Section.EqualIgnorCase(ctx.Section))
            {
                //上下文改变，重新设置链接
                _context = ctx.Clone();
                ClearAccess();
            }
            return MainAccessor.ExecuteNonQuery(strSQL, parameters);
        }
        public static DataSet ExecuteSproc(Context ctx, string procName,string[] tableNames, params SqlParameter[] parameters)
        {
            if (_context == null)
            {
                _context = ctx.Clone();
            }
            else if (!_context.AppConfigFilePath.EqualIgnorCase(ctx.AppConfigFilePath) ||
                !_context.Section.EqualIgnorCase(ctx.Section))
            {
                //上下文改变，重新设置链接
                _context = ctx.Clone();
                ClearAccess();
            }
            return SqlAccessor.ExecuteSproc(procName, tableNames, parameters);
        }
        public static DataSet ExecuteQuery(Context ctx, string procName, string[] tableNames, params SqlParameter[] parameters)
        {
            if (_context == null)
            {
                _context = ctx.Clone();
            }
            else if (!_context.AppConfigFilePath.EqualIgnorCase(ctx.AppConfigFilePath) ||
                !_context.Section.EqualIgnorCase(ctx.Section))
            {
                //上下文改变，重新设置链接
                _context = ctx.Clone();
                ClearAccess();
            }
            return SqlAccessor.ExecuteQuery(procName, tableNames, parameters);
        }
        public static int ExcuteNoQuery(Context ctx,string sql, params SqlParameter[] parameters)
        {
            if (_context == null)
            {
                _context = ctx.Clone();
            }
            else if (!_context.AppConfigFilePath.EqualIgnorCase(ctx.AppConfigFilePath) ||
                !_context.Section.EqualIgnorCase(ctx.Section))
            {
                //上下文改变，重新设置链接
                _context = ctx.Clone();
                ClearAccess();
            }
            return SqlAccessor.ExcuteNoQuery(sql, parameters);
        }
        public static void BulkInsert(Context ctx, BatchInsertParam parameter)
        {
            if (_context == null)
            {
                _context = ctx.Clone();
            }
            else if (!_context.AppConfigFilePath.EqualIgnorCase(ctx.AppConfigFilePath) ||
                !_context.Section.EqualIgnorCase(ctx.Section))
            {
                //上下文改变，重新设置链接
                _context = ctx.Clone();
                ClearAccess();
            }
            SqlAccessor.BulkInsert(parameter);
        }
        private static void ClearAccess()
        {
            _xpoAccessor = null;
            _sqlAccessor = null;
        }
    }
}
