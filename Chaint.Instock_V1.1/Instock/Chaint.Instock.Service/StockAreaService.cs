
using Chaint.Common.Core;
using Chaint.Common.Data;
using Chaint.Common.Interface.Business;
namespace Chaint.Instock.Service
{
    public class StockAreaService:IStockAreaService
    {
        public OperateResult GetStockAreaPlan(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT PLN.FNUMBER, PLN.FNAME FROM T_AUTOSCAN_STOCKAREAPLAN PLN ORDER BY PLN.FID DESC";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
    }
}
