using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IAuthService _auth;
        private AppSettings _appSettings;
        public IdentityService(IOptions<AppSettings> appSettings, IAuthService auth)
        {
            _appSettings = appSettings.Value;
            _auth = auth;
        }
        public async Task<string> GetIdentifier(int claimId)
        {
            return "unknown";
        }
        public ApplicationUser AuthenticateCustomer(Customer customer, string password)
        {
            if (!string.IsNullOrEmpty(customer?.Password) && SecurePasswordHasher.Verify(password, customer.Password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var expiry = DateTime.UtcNow.AddDays(7);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, customer.ID.ToString())
                    }),
                    Expires = expiry,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = new AccessToken
                {
                    access_token = tokenHandler.WriteToken(token),
                    token_type = "",
                    expires_in = expiry.ToString()
                };
                _auth.SetHttpOnlyJWTCookie(accessToken);
                return new ApplicationUser
                {
                    authenticated = true
                };
            }

            return new ApplicationUser();
        }
    }
}
