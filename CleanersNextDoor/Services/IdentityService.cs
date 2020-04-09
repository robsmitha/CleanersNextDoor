using Domain.Entities;
using Domain.Utilities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanersNextDoor.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IAuthenticationService _auth;
        private readonly ICleanersNextDoorContext _context;
        private IAppSettings _appSettings;
        public IdentityService(IOptions<AppSettings> appSettings, 
            IAuthenticationService auth, 
            ICleanersNextDoorContext context)
        {
            _appSettings = appSettings.Value;
            _auth = auth;
            _context = context;
        }

        public IApplicationUser AuthenticateCustomer(Customer customer, string password)
        {
            //todo: verify w customer.secret
            var authenticated = !string.IsNullOrEmpty(customer?.Password) 
                && SecurePasswordHasher.Verify(password, customer.Password) 
                ? IssueToken(customer)
                : false;
            return new ApplicationUser(authenticated);
        }

        public async Task<IApplicationUser> RefreshToken(IAccessToken accessToken)
        {
            try
            {
                var claimsPrincipal = ValidateTokenClaimsPrincipal(accessToken.access_token);
                var id = GetClaimFromPrincipal<int>(claimsPrincipal, ClaimTypes.NameIdentifier);
                if (id != default)
                {
                    var customer = await _context.Customers
                        .FindAsync(id);

                    if (customer?.ID > 0)
                        return new ApplicationUser(IssueToken(customer));
                }
            }
            catch (Exception) { }
            _auth.SetAuthentication();
            return new ApplicationUser();
        }

        private bool IssueToken(Customer customer)
        {
            try
            {
                var expires = DateTime.UtcNow.AddDays(7);

                //todo: GetBytes on customer.Secret 
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, customer.ID.ToString()),
                    new Claim(ClaimTypes.Email, customer.Email.ToString()),
                    new Claim(ClaimTypes.MobilePhone, customer.Phone.ToString()),
                };

                var token = new JwtSecurityToken(_appSettings.JwtIssuer,
                  _appSettings.JwtIssuer,
                  claims: claims,
                  expires: expires,
                  signingCredentials: credentials);

                var jwtToken = new JwtSecurityTokenHandler()
                    .WriteToken(token);

                var accessToken = new AccessToken
                {
                    access_token = jwtToken,
                    token_type = "",
                    expires_in = expires.ToString()
                };

                _auth.SetAuthentication(accessToken, claims);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        private T GetClaimFromPrincipal<T>(ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal?.Claims
                .FirstOrDefault(x => x.Type == claimType);
            return !string.IsNullOrEmpty(claim?.Value) && claim.Value.GetType() != typeof(T)
                ? (T)Convert.ChangeType(claim.Value, typeof(T))
                : default;
        }

        private ClaimsPrincipal ValidateTokenClaimsPrincipal(string jwtToken)
        {
            try
            {
                IdentityModelEventSource.ShowPII = true;

                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = _appSettings.JwtIssuer,
                    ValidIssuer = _appSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret))
                };

                var principal = new JwtSecurityTokenHandler()
                    .ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
