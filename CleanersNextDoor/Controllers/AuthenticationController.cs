using System.Text.Json;
using System.Threading.Tasks;
using CleanersNextDoor.Services;
using Domain.Utilities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using System.Threading;

namespace CleanersNextDoor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _auth;
        public AuthenticationController(IAuthenticationService auth)
        {
            _auth = auth;
        }

        [HttpPost("Authorize")]
        [AllowAnonymous]
        public async Task<IApplicationUser> Authorize()
        {
            var token = HttpContext.Request.Cookies["access_token"];
            var authenticated = HttpContext.Session.Get<bool>("authenticated");
            if (!authenticated && token != null)
            {
                var accessToken = JsonSerializer.Deserialize<AccessToken>(token);
                return await _auth.RefreshToken(accessToken);
            }
            return new ApplicationUser(authenticated);
        }

        [HttpPost("SignOut")]
        public ActionResult<bool> SignOut()
        {
            _auth.ClearAuthentication();
            return true;
        }
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IApplicationUser> SignIn(Customer data)
        {
            return await _auth.AuthenticateCustomer(data.Email, data.Password);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IApplicationUser> SignUp(Customer data)
        {
            var cancellationToken = new CancellationToken();
            return await _auth.CreateCustomer(data, cancellationToken);
        }
    }
}