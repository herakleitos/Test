using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    [Route("[controller]")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IEnumerable<string> Do(int id ,[FromBody]People p)
        {
            string body  = Request.Body.ToString();
            return new string[] { "test1", "test2" };
        }
    }
    public class People
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}