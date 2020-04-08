using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    public interface IAuthenticationService
    {
        int ClaimID { get; }
        string UniqueIdentifier { get; }
        string MobilePhone { get; }
        void SetAuthentication(AccessToken accessToken = null, Claim[] claims = null);
    }
}
