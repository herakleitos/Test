using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi_demo.Demos
{
    public class Demo:IDemo
    {
        public string GetInfo()
        {
            return "通过依赖注入实例化的类";
        }
    }
}