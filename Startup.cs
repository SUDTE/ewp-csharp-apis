using EwpApi.Constants;
using EwpApi.Dto;
using EwpApi.Filters;
using EwpApi.Formatters;
using EwpApi.Service.Builders;
using EwpApi.UpdaterService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EwpApi
{
    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            var path = Directory.GetCurrentDirectory();
            string logPath = ($"{path}\\Logs\\Log.txt");


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IZTECH EWP API",
                    Description = "An EWP API that implemented in C# as ASP.NET Core Web API",                    
                    Contact = new OpenApiContact
                    {
                        Name = "IT Department",
                        Email = "bidb@iyte.edu.tr",
                        Url = new Uri("https://bidb.iyte.edu.tr/en/home-page/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LGPL",
                        Url = new Uri("https://en.wikipedia.org/wiki/GNU_Lesser_General_Public_License"),
                    }
                });
                c.OperationFilter<SwaggerHeaderFilter>();

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);                

            });

            
                      
            services.AddControllers();
            services.AddControllers(config =>
            {
                config.Filters.Add(new HttpContextFilter());
            });

            //services.AddHostedService<WorkerService>();
            services.AddScoped<ResponseBuilderFactory>();

            
            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new XmlSerializerOutputFormatterNamespace());
            }).AddXmlSerializerFormatters();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");


            app.UseStatusCodePages(async codeContext =>
            {
                var httpContext = codeContext.HttpContext;

                Log.Information("Error Page Redirect:" + httpContext.Response.StatusCode.ToString());

                string message = "";

                if (httpContext.Response.StatusCode == StatusCodes.Status405MethodNotAllowed)
                {
                   message = "Method not allowed";
                }

                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    message = "Method not found";
                }


                httpContext.Response.StatusCode = (int)httpContext.Response.StatusCode;
                Dictionary<string, List<string>> errorParameters = new Dictionary<string, List<string>>();
                errorParameters.Add(Constants.ErrorParameters.DeveloperMessage.ToString(), new List<string>() { message });
                IResponse errorResponse = new ErrorResponseBuilder().Build(httpContext.Request, httpContext.Response, errorParameters);
                await errorResponse.WriteXmlBody(httpContext.Response);
               

            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseLoggerMiddleware();

            //app.UseCompressionMiddleware();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IZTECH EWP APIs v1");                
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
