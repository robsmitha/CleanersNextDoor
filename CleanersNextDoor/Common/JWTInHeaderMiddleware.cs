using CleanersNextDoor.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanersNextDoor.Common
{
    public class JWTInHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authenticationCookieName = "access_token";
            var cookie = context.Request.Cookies[authenticationCookieName];
            if (cookie != null && !string.IsNullOrEmpty(cookie))
            {
                var token = JsonSerializer.Deserialize<AccessToken>(cookie);
                if (!string.IsNullOrWhiteSpace(token?.access_token) 
                    && DateTime.TryParse(token.expires_in, out var expiry) 
                    && expiry > DateTime.Now)
                {
                    context.Request.Headers.Append("Authorization", "Bearer " + token.access_token);
                }
                else
                {  
                    //remove cookie if expired or invalid
                    context.Response.Cookies.Delete(authenticationCookieName);
                }
            }

            await _next.Invoke(context);
        }
    }
    public static class JWTInHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTInHeader(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTInHeaderMiddleware>();
        }
    }
}
