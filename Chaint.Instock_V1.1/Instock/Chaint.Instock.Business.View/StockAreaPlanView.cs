
using Chaint.Common.Core;
using Chaint.Common.Entity;
using Chaint.Instock.Business.Model;
namespace Chaint.Instock.Business.View
{
    public class StockAreaPlanView : AbstractBillView
    {
        public StockAreaPlanView(Context ctx) : base(ctx)
        {
            this.Model = new StockAreaPlanModel(ctx);
        }
    }
}
