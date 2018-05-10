using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
using Chaint.Instock.Business.View;
namespace Chaint.Instock.Business.Controler
{
    public class DistributionControler : AbstractControler
    {
        public DistributionControler(DistributionView view) : base(view)
        {
            this.AddPluIn(new DistributionPlugIn(view));
        }
    }
}
