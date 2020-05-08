using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class StripePublicKey : IStripePublicKey
    {
        public string key { get; set; }
        public StripePublicKey(string key)
        {
            this.key = key;
        }
    }
}