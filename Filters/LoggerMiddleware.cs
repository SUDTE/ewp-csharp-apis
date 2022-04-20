using EwpApi.Dto;
using EwpApi.Helper;
using EwpApi.Service.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Filters
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }
    }


    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string requestBody = await BodyReader.ReadRequestBody(context.Request);
            Log.Information($"-----Request Body: {requestBody}");
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                Log.Error(e, e.Message);
                context.Response.StatusCode = (int)500;
                Dictionary<string, List<string>> errorParameters = new Dictionary<string, List<string>>();
                errorParameters.Add(Constants.ErrorParameters.DeveloperMessage.ToString(), new List<string>() { e.Message });
                IResponse errorResponse = new ErrorResponseBuilder().Build(context.Request, context.Response, errorParameters);
                await errorResponse.WriteXmlBody(context.Response);
            }
            

        }
    }
}
