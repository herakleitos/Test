using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI
{
    public static class PreRouteHandler
    {
        public static void HttpPreRoute(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(IHttpActionInvoker), new HttpWebApiControllerActionInvoker());
            config.Services.Replace(typeof(IHttpControllerSelector), new HttpNotFoundDefaultHttpControllerSelector(config));
            config.Services.Replace(typeof(IHttpActionSelector), new HttpNotFoundControllerActionSelector());
        }
    }

    public class HttpNotFoundDefaultHttpControllerSelector : DefaultHttpControllerSelector
    {
        JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
        public HttpNotFoundDefaultHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor decriptor = null;
            try
            {
                decriptor = base.SelectController(request);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                var result = new OutResult() { code = OutCode.失败, msg = "无效请求" };
                if (code == HttpStatusCode.NotFound || code == HttpStatusCode.MethodNotAllowed)
                {
                    ex.Response.Content = new ObjectContent(typeof(OutResult), result, formatter);
                }
                ex.Response.StatusCode = HttpStatusCode.OK;
                throw;
            }
            return decriptor;
        }


    }

    public class HttpNotFoundControllerActionSelector : ApiControllerActionSelector
    {
        JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            HttpActionDescriptor decriptor = null;
            try
            {
                decriptor = base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                var result = new OutResult() { code = OutCode.失败, msg = "无效请求" };
                if (code == HttpStatusCode.NotFound || code == HttpStatusCode.MethodNotAllowed)
                {
                    ex.Response.Content = new ObjectContent(typeof(OutResult), result, formatter);
                }
                ex.Response.StatusCode = HttpStatusCode.OK;
                throw ex;
            }
            return decriptor;
        }
    }

    public class HttpWebApiControllerActionInvoker : ApiControllerActionInvoker
    {
        JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
        public override Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var responseMessage = base.InvokeActionAsync(actionContext, cancellationToken);

            if (responseMessage.Exception != null)
            {
                var baseException = responseMessage.Exception.InnerExceptions[0];
                var result = new OutResult() { code = OutCode.失败, msg = "无效请求" };

                if (baseException is TimeoutException)
                {
                    result.code = OutCode.请求超时;
                    result.msg = "请求超时";
                }

                return Task.Run(() => new HttpResponseMessage()
                {
                    Content = new ObjectContent(typeof(OutResult), result, formatter),
                    StatusCode = HttpStatusCode.OK
                }, cancellationToken);
            }
            return responseMessage;
        }
    }
}
