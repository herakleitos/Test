using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XXZX.BL.Core.EF;
using XXZX.GCJS.Common.Utils;
using XXZX.GCJS.Model.Entity;

namespace XXZX.GCJS.Controller.Filter
{
    public class MyActionAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //没有发生异常时，记录正常的访问日志
            if (filterContext.Exception == null)
            {
                var result = (JsonResult)filterContext.Result;
                TbApiRequestLog log = new TbApiRequestLog();
                log.Api = filterContext.HttpContext.Request.Url.ToString();
                log.AccessToken = filterContext.HttpContext.Request.QueryString.Get("access_token");
                log.ApiParams = CommonFunc.ReadToEnd(filterContext.RequestContext.HttpContext.Request.InputStream);
                log.RequestTime = filterContext.HttpContext.Timestamp;
                log.ApiReturn = Newtonsoft.Json.JsonConvert.SerializeObject(result.Data);
                Logger.WriteRequestLog(log);
            }
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }
    }
}
