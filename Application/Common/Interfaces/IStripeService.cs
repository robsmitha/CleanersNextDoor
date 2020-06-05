namespace Application.Common.Interfaces
{
    public interface IStripeService
    {
        IStripePublicKey StripePublicKey();
        IStripeClientSecret StripeClientSecret(string customerId);
        Stripe.Customer CreateCustomer();
        Stripe.PaymentMethod GetPaymentMethod(string paymentMethodId);
        Stripe.PaymentMethod DetachPaymentMethod(string paymentMethodId);

        /// Amount intended to be collected by this PaymentIntent. 
        /// A positive integer representing how much to charge in the smallest currency unit 
        /// (e.g., 100 cents to charge $1.00 or 100 to charge ¥100, a zero-decimal currency). 
        /// The minimum amount is $0.50 US or equivalent in charge currency. 
        /// The amount value supports up to eight digits (e.g., a value of 99999999 for a USD charge of $999,999.99).
        Stripe.PaymentIntent CreatePaymentIntent(int orderId, long centAmount);
        Stripe.PaymentIntent CreatePaymentIntent(Stripe.PaymentIntentCreateOptions options);
    }
}
