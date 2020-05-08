using Application.Common.Interfaces;
using System;

namespace Infrastructure.Identity
{
    public class AccessToken : IAccessToken
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public DateTime expires_at { get; set; }
    }
}
