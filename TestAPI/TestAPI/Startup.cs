using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Cors;
using TestAPI.Middlewares;

namespace TestAPI
{
    /// <summary>
    /// asp.net core webapi 的重点 1. 路由，2.过滤器Filters，3.中间件Middleware,4.依赖注入
    /// </summary>
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //此方法也可以在Program.cs等价实现
        //通过.ConfigureServices(services=>{具体方法});
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => {
                //options.Filters.Add();添加过滤器
                })
                .AddJsonOptions(options =>//使用IMvcBuilder 配置Json序列化处理
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                });
            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });
            //依赖注入,或者也可以在Program.cs 中注册，具体方法查看Program.cs代码。
            services.AddScoped<IDeviation, Deviation>();
            services.AddScoped<ICalculate,Calculate>();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //日志
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //使用中间件，处理全局事务
            //app.UseMiddleware(typeof(TestMiddlewares));
            //app.UseTestMid();


            app.UseStaticFiles();
            app.UseMvc();

            //app.UseCors(builder =>
            //    builder.WithOrigins("http://example.com")
            //    .AllowAnyHeader()
            //    );

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //       name: "default",
            //       template: "api/{controller}/{action}/{id?}",
            //       defaults: new { controller = "Home", action = "Index" });
            //});
        }
    }
}
