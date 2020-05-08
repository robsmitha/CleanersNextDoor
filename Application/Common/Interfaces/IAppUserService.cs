using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IAppUserService
    {
        /// <summary>
        /// CustomerID or UserID of current authenticated app user
        /// </summary>
        int ClaimID { get; }

        /// <summary>
        /// Email (Customer) or Username (User) of current authenticated app user
        /// </summary>
        string UniqueIdentifier { get; }

        /// <summary>
        /// Mobile phone of current authenticated app user
        /// </summary>
        string MobilePhone { get; }

        /// <summary>
        /// Sets/Replaces identity/authentication values in Cookies, Session and Claims
        /// </summary>
        /// <param name="accessToken">jwt access token</param>
        /// <param name="claims">claims to refresh the current principle</param>
        void SetIdentity(IAccessToken accessToken = null, Claim[] claims = null);
    }
}
