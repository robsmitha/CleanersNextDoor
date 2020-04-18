using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System;
using System.Text.Json;
using System.Threading;
using Infrastructure.Identity;
using Domain.Utilities;
using Microsoft.Extensions.Options;
using Infrastructure.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;

namespace CleanersNextDoor.Services
{
    public class AuthenticationSerivce : IAuthenticationService
    {
        private readonly ICleanersNextDoorContext _context;
        private readonly IIdentityService _identity;
        private IAppSettings _appSettings;

        public AuthenticationSerivce(IOptions<AppSettings> appSettings,
            IIdentityService identity,
            ICleanersNextDoorContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _identity = identity;
        }

        public async Task<IApplicationUser> AuthenticateCustomer(string email, string password)
        {
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
            return VerifyCustomer(customer, password);
        }

        public async Task<IApplicationUser> CreateCustomer(Customer model, CancellationToken cancellationToken)
        {
            //TODO: generate customer secret
            var customer = new Customer
            {
                Name = model.Name,
                Email = model.Email,
                //TODO: hash w customer.secret
                Password = SecurePasswordHasher.Hash(model.Password),
                Phone = model.Phone
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return VerifyCustomer(customer, model.Password);
        }

        private IApplicationUser VerifyCustomer(Customer customer, string password)
        {
            //TODO: verify w customer.secret
            var authenticated = !string.IsNullOrEmpty(customer?.Password)
                && SecurePasswordHasher.Verify(password, customer.Password)
                ? IssueToken(customer)
                : false;
            return new ApplicationUser(authenticated);
        }

        public void ClearAuthentication()
        {
            _identity.SetIdentity();
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

            _identity.SetIdentity();

            return new ApplicationUser();
        }

        private bool IssueToken(Customer customer)
        {
            try
            {
                var expires = DateTime.UtcNow.AddDays(7);

                //TODO: GetBytes on customer.Secret 
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

                _identity.SetIdentity(accessToken, claims);

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
