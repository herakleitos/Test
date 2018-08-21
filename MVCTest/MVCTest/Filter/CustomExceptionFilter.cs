using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Filter
{
    public class CustomExceptionFilter:FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
          //process exception , like log etc.
        }
    }
}