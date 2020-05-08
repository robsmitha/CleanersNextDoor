using CleanersNextDoor.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;

namespace CleanersNextDoor.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Unique int PK of current user
        /// </summary>
        public int ClaimID => int.TryParse(
            _httpContextAccessor?
            .HttpContext
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
            out var @int)
            ? @int
            : 0;

        /// <summary>
        /// Unique string identifier of current user
        /// </summary>
        public string UniqueIdentifier => _httpContextAccessor?
            .HttpContext
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        /// <summary>
        /// Mobile phone of current user
        /// </summary>
        public string MobilePhone => _httpContextAccessor?
            .HttpContext
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)?.Value;

        public void SetIdentity(IAccessToken accessToken = null, Claim[] claims = null)
        {
            if (_httpContextAccessor?.HttpContext == null) return;

            //remove previous if exists
            _httpContextAccessor
               .HttpContext
               .Response
               .Cookies.Delete("access_token");

            //set clear authenticated flag
            _httpContextAccessor
                .HttpContext
                .Session
                .Set("authenticated", false);

            if (accessToken != null)
            {
                //add or replace token
                _httpContextAccessor
                   .HttpContext
                   .Response
                   .Cookies.Append("access_token", JsonSerializer.Serialize(accessToken),
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = Convert.ToDateTime(accessToken.expires_at)
                    });

                //reset authenticated flag
                _httpContextAccessor
                    .HttpContext
                    .Session
                    .Set("authenticated", true);

                if (claims != null)
                {
                    //add to new identity claims
                    var identity = new ClaimsIdentity(claims);
                    var principal = new ClaimsPrincipal(identity);
                    _httpContextAccessor.HttpContext.User = principal;
                    Thread.CurrentPrincipal = principal;
                }
            }
        }
    }
}
