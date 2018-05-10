using System;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.App;
using Chaint.Common.Interface.Business;
using Chaint.Common.Core.Const;
namespace Chaint.Instock.ServiceHelper
{
    public class StockInAutoScanServiceHelper
    {
        public static OperateResult GetDefaultDisplay(Context ctx, string dispType)
        {
            OperateResult result = new OperateResult();
            IStockInAutoScan service
                = AppServiceContext.GetService<IStockInAutoScan>(ctx);
            try
            {
                ctx.Section = Const_ConfigSection.WMSSection;
                result = service.GetDefaultDisplay(ctx, dispType);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
            }
            finally
            {
                ctx.Section = Const_ConfigSection.MainSection;
            }
            return result;
        }
        public static OperateResult GetFactoryOrg(Context ctx, bool isChoose, bool isLocal)
        {
            OperateResult result = new OperateResult();
            IStockInAutoScan service
                = AppServiceContext.GetService<IStockInAutoScan>(ctx);
            try
            {
                ctx.Section = Const_ConfigSection.WMSSection;
                result = service.GetFactoryOrg(ctx, isChoose, isLocal);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
            }
            finally
            {
                ctx.Section = Const_ConfigSection.MainSection;
            }
            return result;
        }
        public static OperateResult GetBusinessType(Context ctx, string businessCode)
        {
            OperateResult result = new OperateResult();
            IStockInAutoScan service
                = AppServiceContext.GetService<IStockInAutoScan>(ctx);
            try
            {
                ctx.Section = Const_ConfigSection.WMSSection;
                result = service.GetBusinessType(ctx, businessCode);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
            }
            finally
            {
                ctx.Section = Const_ConfigSection.MainSection;
            }
            return result;
        }
        public static OperateResult GetShiftInfo(Context ctx, string shiftName)
        {
            OperateResult result = new OperateResult();
            IStockInAutoScan service
                = AppServiceContext.GetService<IStockInAutoScan>(ctx);
            try
            {
                ctx.Section = Const_ConfigSection.WMSSection;
                result = service.GetShiftInfo(ctx, shiftName);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result.IsSuccess = false;
                result.Message = string.Format("发生错误:{0}", ex.Message);
            }
            finally
            {
                ctx.Section = Const_ConfigSection.MainSection;
            }
            return result;
        }
    }
}
