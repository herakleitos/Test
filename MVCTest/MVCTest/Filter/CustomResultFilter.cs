using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Filter
{
    public class CustomResultFilter : FilterAttribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        { }
        public void OnResultExecuting(ResultExecutingContext filterContext)
        { }
    }
}