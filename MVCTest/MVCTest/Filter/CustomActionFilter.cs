using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Filter
{
    public class CustomActionFilter:FilterAttribute,IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        { }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        { }
    }
}