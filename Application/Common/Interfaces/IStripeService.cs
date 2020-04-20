using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IStripeService
    {
        IStripePublicKey StripePublicKey();
        IStripeClientSecret StripeClientSecret(string customerId);
        Stripe.Customer CreateCustomer();
        Stripe.PaymentMethod GetPaymentMethod(string paymentMethodId);
        Stripe.PaymentMethod DetachPaymentMethod(string paymentMethodId);
    }
}
