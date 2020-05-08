using System.Text.Json;
using System.Threading.Tasks;
using CleanersNextDoor.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using System.Threading;
using CleanersNextDoor.Common;
using Application.Common.Interfaces;

namespace CleanersNextDoor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityService _identity;
        public AuthenticationController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpPost("Authorize")]
        [AllowAnonymous]
        public async Task<IAppUser> Authorize()
        {
            var token = HttpContext.Request.Cookies["access_token"];
            var authenticated = HttpContext.Session.Get<bool>("authenticated");
            if (!authenticated && token != null)
            {
                var accessToken = JsonSerializer.Deserialize<AccessToken>(token);
                return await _identity.RefreshToken(accessToken);
            }
            return new AppUser(authenticated);
        }

        [HttpPost("SignOut")]
        public ActionResult<bool> SignOut()
        {
            _identity.ClearAuthentication();
            return true;
        }
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IAppUser> SignIn(Customer data)
        {
            return await _identity.AuthenticateCustomer(data.Email, data.Password);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IAppUser> SignUp(Customer data)
        {
            var cancellationToken = new CancellationToken();
            return await _identity.CreateCustomer(data, cancellationToken);
        }
    }
}