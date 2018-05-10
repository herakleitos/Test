using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.Core.Sql;
using DevExpress.DataAccess.Sql;
using Chaint.Common.App;
using Chaint.Common.Interface;
namespace Chaint.Common.ServiceHelper
{
    public static class DBAccessServiceHelper
    {
        public static OperateResult ExcuteNonQuery(Context ctx, string sql, params QueryParameter[] parameters)
        {
            OperateResult result = new OperateResult();
            IDBAccessService service
               = AppServiceContext.GetService<IDBAccessService>(ctx);
            try
            {
                result = service.ExcuteNonQuery(ctx, sql, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
                return result;
            }
        }
        public static OperateResults ExcuteQuery(Context ctx, string sql,string[] tableNames, 
            params SqlParameter[] parameters)
        {
            OperateResults result = new OperateResults();
            IDBAccessService service
               = AppServiceContext.GetService<IDBAccessService>(ctx);
            try
            {
                result = service.ExecuteQuery(ctx, sql,tableNames, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
                return result;
            }
        }
        public static OperateResult ExecuteQueryWithDataTable(Context ctx, string sql,
           params SqlParameter[] parameters)
        {
            OperateResult result = new OperateResult();
            IDBAccessService service
               = AppServiceContext.GetService<IDBAccessService>(ctx);
            try
            {
                result = service.ExecuteQueryWithDataTable(ctx, sql, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
                return result;
            }
        }
        public static OperateResult ExcuteQuery(Context ctx, string sql, params QueryParameter[] parameters)
        {
            OperateResult result = new OperateResult();
            IDBAccessService service
               = AppServiceContext.GetService<IDBAccessService>(ctx);
            try
            {
                result = service.ExcuteQuery(ctx, sql, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
                return result;
            }
        }
        public static OperationResult ExcuteWithTransaction(Context ctx, List<ExcuteObject> objs)
        {
            OperationResult result = new OperationResult();
            IDBAccessService service
               = AppServiceContext.GetService<IDBAccessService>(ctx);
            try
            {
                result = service.ExcuteWithTransaction(ctx, objs);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
                return result;
            }
        }
        public static OperateResult BulkInsert(Context ctx, BatchInsertParam parameter)
        {
            OperateResult result = new OperateResult();
            IDBAccessService service
               = AppServiceContext.GetService<IDBAccessService>(ctx);
            try
            {
                result = service.BulkInsert(ctx, parameter);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
                return result;
            }
        }
    }
}
