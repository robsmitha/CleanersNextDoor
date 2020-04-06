using Domain.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IIdentityService
    {
        ApplicationUser AuthenticateCustomer(Customer customer, string password);
        Task<string> GetIdentifier(string claimId);
    }
}
