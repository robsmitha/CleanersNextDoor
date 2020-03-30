using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {

        public IdentityService()
        {
            //todo: get user
        }

        public async Task<string> GetIdentifier(int claimId)
        {
            return await Task.FromResult(claimId.ToString());
        }
    }
}
