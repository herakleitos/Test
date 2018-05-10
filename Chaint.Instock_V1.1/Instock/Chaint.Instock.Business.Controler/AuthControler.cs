using Chaint.Common.BaseControler;
using Chaint.Instock.Business.PlugIns;
using Chaint.Instock.Business.View;

namespace Chaint.Instock.Business.Controler
{
    public class AuthControler : AbstractControler
    {
        public AuthControler(AuthView view) : base(view)
        {
            this.AddPluIn(new AuthPlugIn(view));
        }
    }
}
