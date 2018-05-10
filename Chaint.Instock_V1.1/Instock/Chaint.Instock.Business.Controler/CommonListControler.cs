using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
using Chaint.Instock.Business.View;
namespace Chaint.Instock.Business.Controler
{
    public class CommonListControler : AbstractControler
    {
        public CommonListControler(CommonListView view) : base(view)
        {
            this.AddPluIn(new CommonListPlugIn(view));
        }
    }
}
