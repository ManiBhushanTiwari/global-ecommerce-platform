using Stripe;

namespace EcommerceBackend.Services
{
    public class StripeService
    {
        public StripeService(IConfiguration config)
        {
            StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
        }

        public PaymentIntent CreatePaymentIntent(decimal amount, string currency = "usd")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" }
            };
            var service = new PaymentIntentService();
            return service.Create(options);
        }
    }
}
