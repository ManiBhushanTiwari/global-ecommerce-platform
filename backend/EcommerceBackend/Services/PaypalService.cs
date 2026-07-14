using PayPalCheckoutSdk.Core;
using PayPalHttp;

namespace EcommerceBackend.Services
{
    public class PayPalService
    {
        private readonly PayPalHttpClient _client;

        public PayPalService(IConfiguration config)
        {
            var clientId = config["PayPal:ClientId"];
            var clientSecret = config["PayPal:ClientSecret"];
            System.Diagnostics.Debug.WriteLine($"PayPal ClientId: {clientId}");
            System.Diagnostics.Debug.WriteLine($"PayPal ClientSecret length: {clientSecret?.Length}");
            var environment = new SandboxEnvironment(clientId, clientSecret);
            _client = new PayPalHttpClient(environment);
        }

        public PayPalHttpClient GetClient() => _client;
    }
}
