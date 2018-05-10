
using Chaint.Common.Core;
using Chaint.Common.Entity;
using Chaint.Instock.Business.Model;
namespace Chaint.Instock.Business.View
{
    public class StockAreaDataView : AbstractBillView
    {
        public StockAreaDataView(Context ctx):base(ctx)
        {
            this.Model = new StockAreaDataModel(ctx);
        }
    }
}
