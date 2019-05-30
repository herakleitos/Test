using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi_demo.Demos;

namespace webapi_demo.Controllers
{ 
    [RoutePrefix("api/demo")]
    public class TestController : ApiController
    {
        protected IDemo demo;

        public TestController(IDemo _demo)
        {
            demo = _demo;
        }

        [HttpGet]
        [Route("say")]
        public IHttpActionResult Say()
        {
            return Ok(demo.GetInfo());
        }
    }
}
