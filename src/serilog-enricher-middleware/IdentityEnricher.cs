using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using IdentityModel;

namespace SerilogEnricher
{
    public class IdentityEnricher : ILogEventEnricher
    {
        private readonly HttpContext _context;
        public const string PropertyName = "User";
        private readonly IdentityEnricherOptions _options;

        public IdentityEnricher(HttpContext context, IdentityEnricherOptions options)
        {
            _context = context;
            _options = options;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var userId = GetUserId(_context.User);
            if (userId != null)
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty(PropertyName, new ScalarValue(userId)));
            }
        }

        internal string GetUserId(ClaimsPrincipal principal)
        {
            return _options.GetUserId(principal);
        }
    }

    public class IdentityEnricherOptions
    {
        //IdentityModel.JwtClaimTypes
        //System.Security.Claims.ClaimTypes

        public static Func<ClaimsPrincipal, string> DefaultFuncForUserId = (claimsPrincipal) => claimsPrincipal?.FindFirst(JwtClaimTypes.Subject)?.Value;

        public IdentityEnricherOptions()
        {
            this.GetUserId = DefaultFuncForUserId;
        }

        public Func<ClaimsPrincipal,string> GetUserId { get; set; }
    }
}