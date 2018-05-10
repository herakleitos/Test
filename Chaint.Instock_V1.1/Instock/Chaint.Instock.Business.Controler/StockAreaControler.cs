using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
using Chaint.Instock.Business.View;
namespace Chaint.Instock.Business.Controler
{
    public class StockAreaControler : AbstractControler
    {
        public StockAreaControler(StockAreaView view) : base(view)
        {
            this.AddPluIn(new StockAreaPlugIn(view));
        }
    }
}
