using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XXZX.BL.Core.EF;
using XXZX.GCJS.Common;
using XXZX.GCJS.Common.Utils;
using XXZX.GCJS.Model.Entity;

namespace XXZX.GCJS.Controller.Filter
{
    public class HandleExceptionAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {

            base.OnException(context);
            //记录异常日志
            TbApiRequestLog log = new TbApiRequestLog();
            log.Api = context.HttpContext.Request.Url.ToString();
            log.AccessToken = context.HttpContext.Request.QueryString.Get("access_token");
            log.ApiParams = CommonFunc.ReadToEnd(context.RequestContext.HttpContext.Request.InputStream);
            log.RequestTime = context.HttpContext.Timestamp;
            Logger.WriteExceptionLog(log, context.Exception.ToString());

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            if (context.Exception.Message.Equals(errorCode.unauthorised.ToString()))
            {
                context.Result = new JsonResult() { Data = new ApiResult(false, errorCode.unauthorised, null) };
            }
            else if (context.Exception.Message.Equals(errorCode.tokenExpired.ToString()))
            {
                context.Result = new JsonResult() { Data = new ApiResult(false, errorCode.tokenExpired, null) };
            }
            else if (context.Exception.Message.Equals(errorCode.invalidToken.ToString()))
            {
                context.Result = new JsonResult() { Data = new ApiResult(false, errorCode.invalidToken, null) };
            }
            else
            {
                context.Result = new JsonResult() { Data = new ApiResult(false, errorCode.exception, null) };
            }
        }
    }
}