using Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IIdentityService
    {
        Task<IApplicationUser> RefreshToken(IAccessToken token);
        IApplicationUser AuthenticateCustomer(Customer customer, string password);
    }
}
