using Chaint.Common.BaseControler;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.PlugIns;
namespace Chaint.Instock.Business.Controler
{
    public class StockInAutoScanControler : AbstractControler
    {
        public StockInAutoScanControler(StockInAutoScanView view) : base(view)
        {
            this.AddPluIn(new StockInAutoScanPlugIn(view));
        }
    }
}
