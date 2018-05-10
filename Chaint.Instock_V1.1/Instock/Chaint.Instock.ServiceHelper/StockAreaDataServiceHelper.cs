using System;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.App;
using Chaint.Common.Interface.Business;

namespace Chaint.Instock.ServiceHelper
{
    public static class StockAreaDataServiceHelper
    {
        public static OperateResult GetStockAreaInfo(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaDataService service
                = AppServiceContext.GetService<IStockAreaDataService>(ctx);
            try
            {
                result = service.GetStockAreaInfo(ctx);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.StackTrace);
                return result;
            }
        }
        public static OperateResult GetProductInfo(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaDataService service
                = AppServiceContext.GetService<IStockAreaDataService>(ctx);
            try
            {
                result = service.GetProductInfo(ctx);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.StackTrace);
                return result;
            }
        }
        public static OperateResults GetStockDataInfo(Context ctx,string product, string paperType, string specification, string[] stock, string[] stockArea,
            DateTime beginDate, DateTime endDate, int[] status)
        {
            OperateResults result = new OperateResults();
            IStockAreaDataService service
                = AppServiceContext.GetService<IStockAreaDataService>(ctx);
            try
            {
                result = service.GetStockDataInfo(ctx, product,paperType,specification, stock,stockArea , beginDate, endDate, status);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.StackTrace);
                return result;
            }
        }
    }
}
