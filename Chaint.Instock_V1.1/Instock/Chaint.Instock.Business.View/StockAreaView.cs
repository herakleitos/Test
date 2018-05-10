using Chaint.Common.Core;
using Chaint.Common.Entity;
using Chaint.Instock.Business.Model;
namespace Chaint.Instock.Business.View
{
    public class StockAreaView : AbstractBillView
    {
        public StockAreaView(Context ctx):base(ctx)
        {
            this.Model = new StockAreaModel(ctx);
        }
    }
}
