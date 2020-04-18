using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates customer by email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IApplicationUser> AuthenticateCustomer(string email, string password);

        /// <summary>
        /// Creates and authenticates a new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IApplicationUser> CreateCustomer(Customer customer, CancellationToken cancellationToken);

        /// <summary>
        /// Refreshes JWT token cookie when user visits site
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IApplicationUser> RefreshToken(IAccessToken token);

        /// <summary>
        /// Clears token/session authentication values
        /// </summary>
        void ClearAuthentication();
    }
}
