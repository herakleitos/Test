using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XXZX.GCJS.Common;
using XXZX.GCJS.Controller.Services;

namespace XXZX.GCJS.Controller.Filter
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute, IAuthorizationFilter
    {
        public errorCode errorCode;
        protected override bool AuthorizeCore(HttpContextBase context)
        {
            base.AuthorizeCore(context);
            string access_token = context.Request.QueryString.Get("access_token");
            WebApiTokenService service = new WebApiTokenService();
            if (string.IsNullOrWhiteSpace(access_token))
            {
                errorCode = errorCode.invalidToken;
                return false;
            }
            string actionName = context.Request.Url.Segments[context.Request.Url.Segments.Count() - 1];
            var result = service.Validate(access_token, actionName);
            if (result.errorCode != errorCode.success)
            {
                errorCode = result.errorCode;
                return false;
            }
            errorCode = errorCode.success;
            return true;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new JsonResult() { Data = new ApiResult(false, errorCode , null) };
        }
    }
}