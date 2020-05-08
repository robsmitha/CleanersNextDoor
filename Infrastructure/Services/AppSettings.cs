using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class AppSettings : IAppSettings
    {
        public string JwtSecret { get; set; }
        public string JwtIssuer { get; set; }
        public string GoogleApiKey { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripePublicKey { get; set; }
    }
}
