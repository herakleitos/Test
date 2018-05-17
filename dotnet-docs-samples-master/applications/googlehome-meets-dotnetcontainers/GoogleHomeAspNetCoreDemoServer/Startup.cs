﻿// Copyright(c) 2018 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License. You may obtain a copy of
// the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations under
// the License.

using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleHomeAspNetCoreDemoServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            AddGoogleExceptionLogging(services);

            AddGoogleTracing(services);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseGoogleExceptionLogging();
            app.UseGoogleTrace();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static void AddGoogleTracing(IServiceCollection services)
        {
            services.AddGoogleTrace(options =>
            {
                options.ProjectId = Program.AppSettings.GoogleCloudSettings.ProjectId;
            });
        }

        private static void AddGoogleExceptionLogging(IServiceCollection services)
        {
            services.AddGoogleExceptionLogging(options =>
            {
                options.ProjectId = Program.AppSettings.GoogleCloudSettings.ProjectId;
                options.ServiceName = Program.AppSettings.GoogleCloudSettings.ServiceName;
                options.Version = Program.AppSettings.GoogleCloudSettings.Version;
            });
        }
    }
}
