using Microsoft.AspNetCore.Authentication;

namespace Lykke.Service.FIXQuotesApi.Services.Middleware.Auth
{
    public sealed class KeyAuthOptions : AuthenticationSchemeOptions
    {
        public string Secret { get; set; }
    }
}
