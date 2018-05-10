using System;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.App;
using Chaint.Common.Interface.Business;
namespace Chaint.Instock.ServiceHelper
{
    public class StockAreaServiceHelper
    {
        public static OperateResult GetStockAreaPlan(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaService service
                = AppServiceContext.GetService<IStockAreaService>(ctx);
            try
            {
                result = service.GetStockAreaPlan(ctx);
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
