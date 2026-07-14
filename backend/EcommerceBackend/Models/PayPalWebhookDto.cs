namespace EcommerceBackend.Models
{
 
        public class PayPalWebhookDto
        {
            public string Id { get; set; }             // Webhook event ID
            public string EventType { get; set; }      // e.g. PAYMENT.CAPTURE.COMPLETED
            public PayPalResource Resource { get; set; }
            public int OrderId => Resource?.Id ?? 0;
        public string Status => Resource?.Status ?? "UNKNOWN";
    }

        public class PayPalResource
        {
            public int? Id { get; set; }             // PayPal order/capture ID
            public string Status { get; set; }         // COMPLETED, PENDING, FAILED
            public Amount Amount { get; set; }
        }

        public class Amount
        {
            public string CurrencyCode { get; set; }
            public string Value { get; set; }
        }
    }

