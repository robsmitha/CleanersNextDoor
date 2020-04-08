using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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
        public IdentityController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpPost("Authorize")]
        [AllowAnonymous]
        public async Task<ApplicationUser> Authorize()
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
    }
}