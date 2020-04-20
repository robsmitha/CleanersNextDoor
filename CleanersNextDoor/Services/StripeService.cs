using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.Extensions.Options;

namespace CleanersNextDoor.Services
{
    public class StripeService : IStripeService
    {
        private IAppSettings _appSettings;

        public StripeService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            Stripe.StripeConfiguration.ApiKey = _appSettings.StripeSecretKey;
        }
        public IStripePublicKey StripePublicKey()
        {
            return new StripePublicKey(_appSettings.StripePublicKey);
        }
        public IStripeClientSecret StripeClientSecret(string customerId)
        {
            var options = new Stripe.SetupIntentCreateOptions
            {
                Customer = customerId,
            };
            var service = new Stripe.SetupIntentService();
            var intent = service.Create(options);
            return new StripeClientSecret(intent.ClientSecret);
        }
        public Stripe.Customer CreateCustomer()
        {
            var options = new Stripe.CustomerCreateOptions { };
            var service = new Stripe.CustomerService();
            return service.Create(options);
        }
        public Stripe.PaymentMethod GetPaymentMethod(string paymentMethodId)
        {
            var service = new Stripe.PaymentMethodService();
            return service.Get(paymentMethodId);
        }
        public Stripe.PaymentMethod DetachPaymentMethod(string paymentMethodId)
        {
            var service = new Stripe.PaymentMethodService();
            return service.Detach(paymentMethodId);
        }
    }
}
