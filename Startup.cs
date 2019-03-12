using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AspDockerSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions{
                ResponseWriter = (context, report) => {
                    context.Response.ContentType = "application/json";

                    var json = new JObject(
                        new JProperty("app", env.ApplicationName),
                        new JProperty("env", env.EnvironmentName)
                    );

                    return context.Response.WriteAsync(json.ToString(Formatting.Indented));
                }
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
