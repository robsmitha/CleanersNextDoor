using Domain.Services.Configuration.Interfaces;

namespace Domain.Services.Configuration.Models
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
