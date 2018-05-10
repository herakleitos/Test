using Chaint.Common.Core;
using Chaint.Common.Entity;
using Chaint.Instock.Business.Model;
namespace Chaint.Instock.Business.View
{
    public class StockView : AbstractBillView
    {
        public StockView(Context ctx) : base(ctx)
        {
            this.Model = new StockModel(ctx);
        }
    }
}
