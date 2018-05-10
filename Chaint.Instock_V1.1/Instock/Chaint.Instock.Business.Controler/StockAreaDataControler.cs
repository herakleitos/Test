using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
using Chaint.Instock.Business.View;
namespace Chaint.Instock.Business.Controler
{
    public class StockAreaDataControler : AbstractControler
    {
        public StockAreaDataControler(StockAreaDataView view) : base(view)
        {
            this.AddPluIn(new StockAreaDataPlugIn(view));
        }
    }
}
