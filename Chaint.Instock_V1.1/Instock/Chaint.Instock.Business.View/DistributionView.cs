
using Chaint.Common.Core;
using Chaint.Common.Entity;
using Chaint.Instock.Business.Model;
namespace Chaint.Instock.Business.View
{
    public class DistributionView : AbstractBillView
    {
        public DistributionView(Context ctx):base(ctx)
        {
            this.Model = new DistributionModel(ctx);
        }
    }
}
