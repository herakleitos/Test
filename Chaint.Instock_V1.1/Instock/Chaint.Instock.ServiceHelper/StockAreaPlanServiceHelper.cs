using System;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.Interface.Business;
using Chaint.Common.App;
namespace Chaint.Instock.ServiceHelper
{
    public class StockAreaPlanServiceHelper
    {
        /// <summary>
        /// 获取产品备注(特殊客户)
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetSpCustomer(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetSpCustomer(ctx);
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
        /// <summary>
        /// 获取纸芯经
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetCoreDiameter(Context ctx)
        {
            OperateResult result  = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetCoreDiameter(ctx);
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
        /// <summary>
        /// 获取产品等级
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetPaperGrade(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetPaperGrade(ctx);
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
        /// <summary>
        /// 获取产品认证
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetPaperCertification(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {

                result = service.GetPaperCertification(ctx);
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
        /// <summary>
        /// 获取包装方式
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="PaperType"></param>
        /// <returns></returns>
        public static OperateResult GetPackType(Context ctx,string PaperType)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetPackType(ctx, PaperType);
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
        /// <summary>
        /// 获取规格
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="PaperType"></param>
        /// <returns></returns>
        public static OperateResult GetSpecification(Context ctx, string PaperType)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetSpecification(ctx, PaperType);
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
        /// <summary>
        /// 获取夹板包装
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetTransportType(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetTransportType(ctx);
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
        /// <summary>
        /// 获取令数
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetReam(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetReam(ctx);
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
        /// <summary>
        /// 获取卷筒直径
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetRollDiameter(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetRollDiameter(ctx);
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
        /// <summary>
        /// 获取卷筒线长
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetRollLength(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetRollLength(ctx);
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
        /// <summary>
        /// 获取产品色相
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetPaperColor(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetPaperColor(ctx);
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
        /// <summary>
        /// 获取专用客户
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetSpecCustomerName(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetSpecCustomerName(ctx);
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
        /// <summary>
        /// 获取专用产品
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetSpecProductName(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetSpecProductName(ctx);
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
        /// <summary>
        /// 获取合格证样式
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetTrademarkStyle(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetTrademarkStyle(ctx);
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
        /// <summary>
        /// 获取计重方式
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetWeightMode(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetWeightMode(ctx);
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
        /// <summary>
        /// 获取令张数
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static OperateResult GetSlidesOfReam(Context ctx)
        {
            OperateResult result = new OperateResult();
            IStockAreaPlanService service
                = AppServiceContext.GetService<IStockAreaPlanService>(ctx);
            try
            {
                result = service.GetSlidesOfReam(ctx);
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
