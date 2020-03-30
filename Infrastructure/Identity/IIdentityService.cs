using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IIdentityService
    {
        Task<string> GetIdentifier(int claimId);
    }
}
