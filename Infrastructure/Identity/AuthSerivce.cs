using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using System.Text.Json;

namespace Infrastructure.Identity
{
    public class AuthSerivce : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthSerivce(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private string _claimdId => _httpContextAccessor?
            .HttpContext
            .User
            .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        public int ClaimID => int.TryParse(_claimdId, out var @int)
                        ? @int
                        : 0;
        public void SetHttpOnlyJWTCookie(AccessToken accessToken)
        {
            var cookie = _httpContextAccessor?
                .HttpContext
                .Request
                .Cookies["access_token"];
            
            if (cookie != null)
            {
                //remove previous if exists
                _httpContextAccessor?
                   .HttpContext
                   .Response
                   .Cookies.Delete("access_token");
            }

            if (accessToken != null)
            {
                //add or replace token
                _httpContextAccessor?
                   .HttpContext
                   .Response
                   .Cookies.Append("access_token", JsonSerializer.Serialize(accessToken),
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = Convert.ToDateTime(accessToken.expires_in)
                    });
            }
        }
    }
}
