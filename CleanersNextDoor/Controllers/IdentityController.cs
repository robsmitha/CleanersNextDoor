using System.Text.Json;
using System.Threading.Tasks;
using CleanersNextDoor.Services;
using Domain.Utilities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanersNextDoor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identity;
        private readonly IAuthenticationService _auth;
        public IdentityController(IIdentityService identity, IAuthenticationService auth)
        {
            _identity = identity;
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
                return await _identity.RefreshToken(accessToken);
            }
            return new ApplicationUser(authenticated);
        }

        [HttpPost("SignOut")]
        public ActionResult<bool> SignOut()
        {
            _auth.SetAuthentication(null);
            return true;
        }
    }
}