using CleanersNextDoor.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;

namespace CleanersNextDoor.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService()
        {

        }

        public int ClaimID { get; set; }
    }
}
