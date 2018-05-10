
using Chaint.Common.Core;

namespace Chaint.Common.Interface.Business
{
    public interface IStockAreaService
    {
        /// <summary>
        /// 获取库区计划
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        OperateResult GetStockAreaPlan(Context ctx);


    }
}
