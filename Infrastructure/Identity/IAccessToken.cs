using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public interface IAccessToken
    {
        string token_type { get; set; }
        string access_token { get; set; }
        string expires_in { get; set; }
    }
}
