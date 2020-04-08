using Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IIdentityService
    {
        Task<ApplicationUser> RefreshToken(AccessToken token);
        ApplicationUser AuthenticateCustomer(Customer customer, string password);
    }
}
