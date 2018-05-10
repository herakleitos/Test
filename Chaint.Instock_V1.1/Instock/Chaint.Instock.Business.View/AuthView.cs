using Chaint.Common.Core;
using Chaint.Common.Entity;
using Chaint.Instock.Business.Model;
namespace Chaint.Instock.Business.View
{
    public class AuthView : AbstractBillView
    {
        public AuthView(Context ctx) : base(ctx)
        {
            this.Model = new AuthModel(ctx);
        }
    }
}
