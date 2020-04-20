using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanersNextDoor.Services
{
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
        public string JwtIssuer { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripePublicKey { get; set; }
    }
}
