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
            if (cookie != null)
            {
                var token = JsonSerializer.Deserialize<AccessToken>(cookie);
                context.Request.Headers.Append("Authorization", "Bearer " + token.access_token);
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
