using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
using Chaint.Instock.Business.View;
namespace Chaint.Instock.Business.Controler
{
    public class StockAreaPlanControler:AbstractControler
    {
        public StockAreaPlanControler(StockAreaPlanView view) : base(view)
        {
            this.AddPluIn(new StockAreaPlanPlugIn(view));
        }
    }
}
