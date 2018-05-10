
using Chaint.Common.Core;
namespace Chaint.Common.Interface.Business
{
    public interface IStockAreaPlanService
    {
        /// <summary>
        /// 获取产品备注(特殊客户)
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetSpCustomer(Context ctx);
        /// <summary>
        /// 获取纸芯径
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetCoreDiameter(Context ctx);

        /// <summary>
        /// 获取产品等级
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetPaperGrade(Context ctx);

        /// <summary>
        /// 获取产品认证
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetPaperCertification(Context ctx);
        /// <summary>
        /// 获取包装方式
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="PaperType"></param>
        /// <returns></returns>
        OperateResult GetPackType(Context ctx, string PaperType);
        OperateResult GetSpecification(Context ctx, string paperType);
        /// <summary>
        /// 获取夹板包装
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetTransportType(Context ctx);
        /// <summary>
        /// 获取令数
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetReam(Context ctx);
        /// <summary>
        /// 获取卷筒直径
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetRollDiameter(Context ctx);
        /// <summary>
        /// 获取卷筒线长
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetRollLength(Context ctx);
        /// <summary>
        /// 获取产品色相
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetPaperColor(Context ctx);
        /// <summary>
        /// 获取专用客户
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetSpecCustomerName(Context ctx);
        /// <summary>
        /// 获取专用产品
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetSpecProductName(Context ctx);
        /// <summary>
        /// 获取专用产品
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetTrademarkStyle(Context ctx);
        /// <summary>
        /// 获取专用产品
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetWeightMode(Context ctx);

        /// <summary>
        /// 获取令张数
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetSlidesOfReam(Context ctx);
    }
}
