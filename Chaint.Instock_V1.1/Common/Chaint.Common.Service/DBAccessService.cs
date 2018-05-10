using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data.SqlClient;
using Chaint.Common.Core;
using Chaint.Common.Core.Sql;
using DevExpress.DataAccess.Sql;
using Chaint.Common.Data;
using Chaint.Common.Interface;
using Chaint.Common.Core.Log;
namespace Chaint.Common.Service
{
    public class DBAccessService : IDBAccessService
    {
        public OperateResult ExcuteNonQuery(Context ctx, string sql, params QueryParameter[] parameters)
        {
            OperateResult result = new OperateResult();
            try
            {
                result.AffectRow = DBService.ExecuteNonQuery(ctx, sql, parameters);
                result.IsSuccess = true;
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
        public OperateResult BulkInsert(Context ctx, BatchInsertParam parameter)
        {
            OperateResult result = new OperateResult();
            try
            {
                DBService.BulkInsert(ctx, parameter);
                result.IsSuccess = true;
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
        public OperateResults ExecuteSproc(Context ctx, string procName, string[] tableNames, params SqlParameter[] parameters)
        {
            OperateResults result = new OperateResults();
            try
            {
                result.ResultData =
                    DBService.ExecuteSproc(ctx, procName, tableNames, parameters);
                result.IsSuccess = true;
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
        public OperateResults ExecuteQuery(Context ctx, string sql, string[] tableNames, params SqlParameter[] parameters)
        {
            OperateResults result = new OperateResults();
            try
            {
                result.ResultData =
                    DBService.ExecuteQuery(ctx, sql, tableNames, parameters);
                result.IsSuccess = true;
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
        public OperateResult ExecuteQueryWithDataTable(Context ctx, string sql, params SqlParameter[] parameters)
        {
            OperateResult result = new OperateResult();
            try
            {
                result.ResultDataTable =
                    DBService.ExecuteQueryWithDataTable(ctx, sql, parameters);
                result.IsSuccess = true;
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
        public OperateResult ExcuteQuery(Context ctx, string sql, params QueryParameter[] parameters)
        {
            OperateResult result = new OperateResult();
            try
            {
                result.ResultTable =
                    DBService.ExecuteQuery(ctx, sql, parameters);
                result.IsSuccess = true;
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
        public OperationResult ExcuteWithTransaction(Context ctx, List<ExcuteObject> objs)
        {
            OperationResult result = new OperationResult();
            result.IsSuccess = true;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (ExcuteObject obj in objs)
                    {
                        OperateResult opResult = new OperateResult();
                        opResult.IsSuccess = true;
                        try
                        {
                            if (obj.BatchInsertParam != null)
                            {//批量插入
                                DBService.BulkInsert(ctx, obj.BatchInsertParam);
                            }
                            else if (obj.Parameters.Count <= 0)
                            {//不带参数的sql
                                DBService.ExcuteNoQuery(ctx, obj.Sql);
                            }
                            else
                            {//带参数的sql
                                DBService.ExcuteNoQuery(ctx, obj.Sql, obj.Parameters.ToArray());
                            }
                        }
                        catch (Exception ex)
                        {
                            opResult.IsSuccess = false;
                            opResult.Message = ex.Message;
                            result.IsSuccess = false;
                            result.Message = ex.Message;
                            Logger.Log(ex);
                            throw ex;
                        }
                        finally
                        {
                            result.ResultFun.Add(opResult);
                        }
                    }
                    scope.Complete();
                }
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
