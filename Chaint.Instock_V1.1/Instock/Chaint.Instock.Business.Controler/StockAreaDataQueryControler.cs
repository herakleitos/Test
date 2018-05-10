using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
using Chaint.Instock.Business.View;
namespace Chaint.Instock.Business.Controler
{
    public class StockAreaDataQueryControler : AbstractControler
    {
        public StockAreaDataQueryControler(StockAreaDataQueryView view) : base(view)
        {
            this.AddPluIn(new StockAreaDataQueryPlugIn(view));
        }
    }
}
