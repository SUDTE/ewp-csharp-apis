using EwpApi.Helper;
using EwpApi.Service;
using EwpApi.Service.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Filters
{

    public static class CompressionMiddlewareExtension
    {
        public static IApplicationBuilder UseCompressionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CompressionMiddleware>();
        }
    }

    public class CompressionMiddleware
    {
        private readonly RequestDelegate _next;

        public CompressionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var acceptEncoding = context.Request.Headers["Accept-Encoding"];
            if (acceptEncoding.ToString().IndexOf("gzip", StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                await _next(context);
                return;
            }

            using (var buffer = new MemoryStream())
            {
                var body = context.Response.Body;
                context.Response.Body = buffer;
                try
                {
                    await _next(context);

                    using (var compressed = new MemoryStream())
                    {
                        using (var gzip = new GZipStream(compressed, CompressionLevel.Optimal, leaveOpen: true))
                        {
                            buffer.Seek(0, SeekOrigin.Begin);
                            await buffer.CopyToAsync(gzip);
                        }

                        if (compressed.Length < buffer.Length)
                        {
                            compressed.Seek(0, SeekOrigin.Begin);
                            var compressedReader = new StreamReader(compressed);
                            var compressedText = compressedReader.ReadToEnd();
                            string digest = "SHA-256=" + RsaHelper.ComputeSha256Hash(compressedText);
                            context.Response.Headers["Digest"] = digest;
                            if (context.Response.Headers.ContainsKey("Signature"))
                                context.Response.Headers["Signature"] = (new EchoResponseBuilder()).GenerateSignatureHeader(context.Response);


                            // write compressed data to response
                            context.Response.Headers.Add("Content-Encoding", new[] { "gzip" });
                            if (context.Response.Headers["Content-Length"].Count > 0)
                            {
                                context.Response.Headers["Content-Length"] = compressed.Length.ToString();
                            }
                            
                            compressed.Seek(0, SeekOrigin.Begin);
                            await compressed.CopyToAsync(body);
                            return;
                        }
                    }

                    // write uncompressed data to response
                    buffer.Seek(0, SeekOrigin.Begin);
                    await buffer.CopyToAsync(body);
                }
                finally
                {
                    context.Response.Body = body;
                }
            }
        }

    }
    
}
