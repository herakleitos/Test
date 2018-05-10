
using Chaint.Common.BaseControler;
using Chaint.Instock.Main.PlugIns;
namespace Chaint.Instock.Main
{
    public class MainFormControler:AbstractControler
    {
        public MainFormControler(MainFormView view):base(view)
        {
            this.AddPluIn(new MainFormPlugIn(view));
        }
    }
}
