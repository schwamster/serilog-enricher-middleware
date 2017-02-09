using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Serilog.Context;

namespace SerilogEnricher
{
    public class SerilogEnricherMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogEnricherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperties(new OperationEnricher(context), new IdentityEnricher(context)))
            {
                await _next.Invoke(context);
            }
        }
    }

    public static class SerilogEnricherMiddlewareExtension
    {
        public static IApplicationBuilder UseSerilogEnricherMiddleware(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<SerilogEnricherMiddleware>();
        }
    }

}