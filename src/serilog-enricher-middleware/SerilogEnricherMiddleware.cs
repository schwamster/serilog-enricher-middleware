using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

namespace SerilogEnricher
{
    public class SerilogEnricherMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly SerilogEnricherMiddlewareOptions _options;

        public SerilogEnricherMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, SerilogEnricherMiddlewareOptions options)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<SerilogEnricherMiddleware>();
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync(_options.Message);
            await _next(context);
            
        }
    }

    public static class SerilogEnricherMiddlewareExtension
    {
        public static IApplicationBuilder UseSerilogEnricherMiddleware(this IApplicationBuilder builder, SerilogEnricherMiddlewareOptions options)
        {
            return builder.UseMiddleware<SerilogEnricherMiddleware>(options);
        }
    }

    public class SerilogEnricherMiddlewareOptions
    {
        public SerilogEnricherMiddlewareOptions()
        {
            Message = "hello from middleware";    
        }

        public string Message { get; set; }
    }
}