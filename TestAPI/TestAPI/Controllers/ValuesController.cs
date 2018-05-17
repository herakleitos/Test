using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    [Route("api/values")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpPut("{name}")]
        [Route("sayhello/{name}")]
        public string SayHello(string name)
        {
            return string.Format("Hello {0}!", name);
        }
        // GET api/values/5
        [HttpPut("{name}")]
        [Route("{name}/say/{words}")]
        public IActionResult Say(string name,string words)
        {
            return Ok(string.Format("{0} says :{1}!", name,words));
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
