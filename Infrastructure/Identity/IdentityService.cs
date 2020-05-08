using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Utilities;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IApplicationDbContext _context;
        private readonly IAppUserService _user;
        private readonly IStripeService _stripe;
        private AppSettings _appSettings;

        public IdentityService(IOptions<AppSettings> appSettings,
            IAppUserService user,
            IApplicationDbContext context,
            IStripeService stripe)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _user = user;
            _stripe = stripe;
        }
        public async Task<IAppUser> AuthenticateCustomer(string email, string password)
        {
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

            return VerifyCustomer(customer, password);
        }

        public async Task<IAppUser> CreateCustomer(Customer model, CancellationToken cancellationToken)
        {
            var sCustomer = _stripe.CreateCustomer();

            //TODO: generate customer secret
            var customer = new Customer
            {
                Name = model.Name,
                Email = model.Email,
                //TODO: hash w customer.secret
                Password = SecurePasswordHasher.Hash(model.Password),
                Phone = model.Phone,
                StripeCustomerID = sCustomer.Id
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return VerifyCustomer(customer, model.Password);
        }

        public IStripeClientSecret GetStripeSecretKey(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            return _stripe.StripeClientSecret(customer.StripeCustomerID);
        }

        private IAppUser VerifyCustomer(Customer customer, string password)
        {
            //TODO: verify w customer.secret
            var authenticated = !string.IsNullOrEmpty(customer?.Password)
                && SecurePasswordHasher.Verify(password, customer.Password)
                ? IssueToken(customer)
                : false;
            return new AppUser(authenticated);
        }

        public void ClearAuthentication()
        {
            _user.SetIdentity();
        }

        public async Task<IAppUser> RefreshToken(IAccessToken accessToken)
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
                        return new AppUser(IssueToken(customer));
                }
            }
            catch (Exception) { }

            _user.SetIdentity();

            return new AppUser();
        }

        private bool IssueToken(Customer customer)
        {
            try
            {
                var expires = DateTime.UtcNow.AddDays(7);

                //TODO: GetBytes on customer.Secret 
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecret));
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
                    expires_at = expires
                };

                _user.SetIdentity(accessToken, claims);

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecret))
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


        public string StripeClientSecret(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            var options = new Stripe.SetupIntentCreateOptions
            {
                Customer = customer.StripeCustomerID,
            };
            var service = new Stripe.SetupIntentService();
            var intent = service.Create(options);
            return intent.ClientSecret;
        }
    }
}
