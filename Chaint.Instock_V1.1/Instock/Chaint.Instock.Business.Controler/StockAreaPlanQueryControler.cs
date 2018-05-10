using Chaint.Common.BaseControler;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.PlugIns;
namespace Chaint.Instock.Business.Controler
{
    public class StockAreaPlanQueryControler : AbstractControler
    {
        public StockAreaPlanQueryControler(StockAreaPlanQueryView view) : base(view)
        {
            this.AddPluIn(new StockAreaPlanQueryPlugIn(view));
        }
    }
}
