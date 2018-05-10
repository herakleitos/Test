using System;
using Chaint.Common.Core;
namespace Chaint.Common.Interface.Business
{
    public interface IStockAreaDataService
    {
        OperateResult GetStockAreaInfo(Context ctx);
        OperateResult GetProductInfo(Context ctx);
        OperateResults GetStockDataInfo(Context ctx,string product,string paperType,string specification,string[] stock,string[] stockArea ,DateTime beginDate, DateTime endDate
            , int[] statusArray);
    }
}
