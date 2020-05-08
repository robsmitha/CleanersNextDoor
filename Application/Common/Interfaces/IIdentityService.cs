using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        /// <summary>
        /// Authenticates customer by email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IAppUser> AuthenticateCustomer(string email, string password);

        /// <summary>
        /// Creates and authenticates a new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IAppUser> CreateCustomer(Customer customer, CancellationToken cancellationToken);

        /// <summary>
        /// Refreshes JWT token cookie when user visits site
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IAppUser> RefreshToken(IAccessToken token);

        /// <summary>
        /// Clears token/session authentication values
        /// </summary>
        void ClearAuthentication();

        IStripeClientSecret GetStripeSecretKey(int customerId);
    }
}
