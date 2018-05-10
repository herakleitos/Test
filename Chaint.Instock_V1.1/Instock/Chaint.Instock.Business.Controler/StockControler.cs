using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
using Chaint.Instock.Business.View;

namespace Chaint.Instock.Business.Controler
{
    public class StockControler : AbstractControler
    {
        public StockControler(StockView view) : base(view)
        {
            this.AddPluIn(new StockPlugIn(view));
        }
    }
}
