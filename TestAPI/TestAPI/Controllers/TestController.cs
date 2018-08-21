using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    [Route("api/test")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost,Route("do/{id}")]
        public async Task<IActionResult> Do(int id,[FromBody]People p)
        {
            byte[] content = new byte[1000];
            await Request.Body.ReadAsync(content,0,1000);
            
            string result = string.Format("FirstName:{0},LastName:{1}",p.FirstName,p.LastName);
            return NotFound();
            //return BadRequest();
            //return Ok(result);
        }
    }
    public class People
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}