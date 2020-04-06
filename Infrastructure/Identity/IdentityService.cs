using Domain.Entities;
using Infrastructure.Data;
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
        private AppSettings _appSettings;
        public IdentityService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public async Task<string> GetIdentifier(string claimId)
        {
            return "unknown";
        }
        public ApplicationUser AuthenticateCustomer(Customer customer, string password)
        {
            if (!string.IsNullOrEmpty(customer?.Password) && SecurePasswordHasher.Verify(password, customer.Password))
            {
                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, customer.ID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var identity = new ApplicationUser
                {
                    ID = customer.ID,
                    Token = tokenHandler.WriteToken(token)
                };
                return identity;
            }

            // return null if user not found
            return null;
        }
    }
}
