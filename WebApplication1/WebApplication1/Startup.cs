#define COMBINE_API_VERSIONS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(ops =>
            {
                // Will return and controllers action that support [ApiVersion("2.0")] attribute
                ops.DefaultApiVersion = new ApiVersion(2, 0);

                // if api-version is not specified, it will route to a Default Api Version
                ops.AssumeDefaultVersionWhenUnspecified = true; 

                // => return Http Reponse api-supported-version: (action supported version) 1.0, 1.5, 2.0
                ops.ReportApiVersions = true;

#if COMBINE_API_VERSIONS

                // Since we want to multiple way to specify api version we use ApiVersionReader.Combine
                // Options how you where you can specify api-version
                ops.ApiVersionReader = ApiVersionReader.Combine(new IApiVersionReader[]
                {
                    // should be specified in query like ?api-version=1.0
                    new QueryStringApiVersionReader("api-version"),
                    // should be specified in query like ?v=1.0
                    new QueryStringApiVersionReader("v"),

                    // should be specified in HTTP Request Headers
                    // X-Version: 1.0
                    new HeaderApiVersionReader("X-Version"),
                    // Api-Version: 1.5
                    new HeaderApiVersionReader("Api-Version")
                });
#else

                // This is by default
                // where you can specify version in query
                // like  https://localhost:5001/api/values?api-version=1.0
                ops.ApiVersionReader = new QueryStringApiVersionReader("api-version");

                // will override QueryStringApiVersionReader
                // 
                 ops.ApiVersionReader = new HeaderApiVersionReader("X-Version");
#endif
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
