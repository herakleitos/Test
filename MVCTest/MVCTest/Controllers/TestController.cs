using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTest.Models;
namespace MVCTest.Controllers
{
    public class TestController : Controller
    {
        public string GetString()
        {
            return "Hello World!";
        }
        public ActionResult GetView()
        {
            Employee e = new Employee();
            e.FirstName = "姓氏";
            e.LastName = "名字";
            e.salary = 15000;
            ViewData["Employee"] = e;
            return View("MyView");
        }
    }
}