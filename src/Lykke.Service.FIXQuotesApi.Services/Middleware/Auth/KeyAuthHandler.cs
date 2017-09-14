using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lykke.Service.FIXQuotesApi.Services.Middleware.Auth
{
    public sealed class KeyAuthHandler : AuthenticationHandler<KeyAuthOptions>
    {
        private const string DefaultHeaderName = "api-key";
        public const string AuthScheme = "CustomScheme";


        public KeyAuthHandler(IOptionsMonitor<KeyAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            Context.Request.Headers.TryGetValue(DefaultHeaderName, out var headerValue);

            var apiKey = headerValue.FirstOrDefault();
            ClaimsIdentity identity;

            if (apiKey != null && apiKey == Options.Secret)
            {
                identity = new ClaimsIdentity("apikey");
            }
            else
            {
                identity = new ClaimsIdentity("anonymous");
            }

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), null, AuthScheme);
            return await Task.FromResult(AuthenticateResult.Success(ticket));



        }
    }
}
