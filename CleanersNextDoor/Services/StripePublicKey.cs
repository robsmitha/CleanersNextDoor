using Infrastructure.Identity;

namespace CleanersNextDoor.Services
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