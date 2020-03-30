using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public interface IAuthenticationService
    {
        public int ClaimID { get; }
    }
}
