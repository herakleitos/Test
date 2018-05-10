using System.Collections.Generic;
using System.Data.SqlClient;
using Chaint.Common.Core;
using Chaint.Common.Core.Sql;
using DevExpress.DataAccess.Sql;
namespace Chaint.Common.Interface
{
    public interface IDBAccessService
    {
        OperateResults ExecuteSproc(Context ctx, string procName, string[] tableNames, params SqlParameter[] parameters);
        OperateResults ExecuteQuery(Context ctx, string sql, string[] tableNames, params SqlParameter[] parameters);
        OperateResult ExcuteNonQuery(Context ctx, string sql, params QueryParameter[] parameters);
        OperateResult BulkInsert(Context ctx, BatchInsertParam parameter);
        OperationResult ExcuteWithTransaction(Context ctx, List<ExcuteObject> objs);
        OperateResult ExcuteQuery(Context ctx, string sql, params QueryParameter[] parameters);
        OperateResult ExecuteQueryWithDataTable(Context ctx, string sql, params SqlParameter[] parameters);
    }
}
