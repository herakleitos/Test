using MVCTest.Filter;
using System.Web;
using System.Web.Mvc;

namespace MVCTest
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //auth
            filters.Add(new CustomAuthFilter());
            //exception
            filters.Add(new CustomExceptionFilter());
            //action
            filters.Add(new CustomActionFilter());
            //result
            filters.Add(new CustomResultFilter());
        }
    }
}
