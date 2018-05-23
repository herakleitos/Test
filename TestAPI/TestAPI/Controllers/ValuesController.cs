using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestAPI.Controllers
{
    [Route("api/values")]
    public class ValuesController : Controller
    {
        private ICalculate _calculate;

        public ValuesController(ICalculate calculate)
        {
            _calculate = calculate;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("sayhello/{name}")]
        public string SayHello(string name)
        {
            return string.Format("Hello {0}!", name);
        }
        // GET api/values/5
        [HttpPost, Route("{name}/say/{words}")]
        public IActionResult Say(string name, string words,[FromBody]string aaa)
        {
            return Ok(string.Format("{0} says :{1}!", name,words));
        }
        [Route("say")]
        [HttpPost]
        public IActionResult Say()
        {
            return Ok(string.Format("{0} says :{1}!", "lily","long time no see"));
        }
        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut,Route("test/{a:int}/{b:int}")]
        public IActionResult test(int a, int b)
        {

            return Ok(string.Format("result is {0}", _calculate.Add(a,b)));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
