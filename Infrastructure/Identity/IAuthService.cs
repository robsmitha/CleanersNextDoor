using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public interface IAuthService
    {
        public string ClaimID { get; }
    }
}
