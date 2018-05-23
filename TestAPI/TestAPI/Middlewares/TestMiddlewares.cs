using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Middlewares
{
    //测试中间件
    public class TestMiddlewares
    {
        private readonly RequestDelegate _next;

        public TestMiddlewares(RequestDelegate next)
        {
            _next = next;
        }
        //请求发起时，首先将请求传递给中间件，请求在多个中间件中按顺序传递，最后传递至controller
        //
        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("Hello World!");
            context.Response.OnCompleted(ResponseCompletedCallback, context);
            await _next(context);
        }
        private Task ResponseCompletedCallback(object obj)
        {
            return Task.FromResult(0);
        }
    }
    public static class VisitLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMid(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMiddlewares>();
        }
    }
}
