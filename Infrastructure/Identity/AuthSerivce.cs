using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public class AuthSerivce : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthSerivce(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ClaimID
        {

            get
            {
                if(_httpContextAccessor?.HttpContext != null)
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                }
                return null;
            }
        }
    }
}
