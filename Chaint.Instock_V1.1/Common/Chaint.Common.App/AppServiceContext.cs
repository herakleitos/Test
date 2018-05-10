using Chaint.Common.Interface;
using Chaint.Common.Core;
namespace Chaint.Common.App
{
    public static class AppServiceContext
    {
        public static T GetService<T>(Context ctx)
        {
            return ServiceFactory.GetService<T>(ctx);
        }
    }
}
 