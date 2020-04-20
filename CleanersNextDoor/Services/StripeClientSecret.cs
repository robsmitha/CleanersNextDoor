using Application.Common.Interfaces;
using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanersNextDoor.Services
{
    public class StripeClientSecret : IStripeClientSecret
    {
        public string secret { get; set; }
        public StripeClientSecret(string secret)
        {
            this.secret = secret;
        }
    }
}
