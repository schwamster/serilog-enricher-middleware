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
        private readonly IdentityEnricherOptions _options;

        public SerilogEnricherMiddleware(RequestDelegate next, IdentityEnricherOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperties(new OperationEnricher(context), new IdentityEnricher(context,_options)))
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

            return app.UseMiddleware<SerilogEnricherMiddleware>(new IdentityEnricherOptions());
        }

        public static IApplicationBuilder UseSerilogEnricherMiddleware(this IApplicationBuilder app, IdentityEnricherOptions identityEnricherOptions)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<SerilogEnricherMiddleware>(identityEnricherOptions);
        }
    }

}