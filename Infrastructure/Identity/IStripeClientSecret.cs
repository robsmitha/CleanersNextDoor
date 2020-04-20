using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public interface IStripeClientSecret
    {
        string secret { get; set; }
    }
}
