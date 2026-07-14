using EcommerceBackend.Models;
using EcommerceBackend.Services;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;

namespace EcommerceBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _service;
        public PaymentsController(PaymentService service) => _service = service;

        [HttpGet] public IActionResult GetAll() => Ok(_service.GetAllPayments());
        [HttpGet("{id}")] public IActionResult GetById(int id) => _service.GetPaymentById(id) is Payment p ? Ok(p) : NotFound();
        [HttpPost] public IActionResult Create(Payment payment) { _service.AddPayment(payment); return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment); }
        [HttpPost("paypal")]
        public async Task<IActionResult> CreatePayPalPayment([FromServices] PayPalService paypalService)
        {
            var client = paypalService.GetClient();

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
        {
            new PurchaseUnitRequest
            {
                AmountWithBreakdown = new AmountWithBreakdown
                {
                    CurrencyCode = "USD",
                    Value = "10.00"
                }
            }
        }
            });

            var response = await client.Execute(request);
            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();


            // ✅ Extract approval link
            var approvalLink = result.Links.FirstOrDefault(l => l.Rel == "approve")?.Href;

            return Ok(new { url = approvalLink, orderId = result.Id });
        }


        //[HttpPost("paypal/capture")]
        //public async Task<IActionResult> CapturePayPalPayment(string orderId, [FromServices] PayPalService paypalService)
        //{
        //    var client = paypalService.GetClient();
        //    var request = new PayPalCheckoutSdk.Orders.OrdersCaptureRequest(orderId);
        //    request.RequestBody(new PayPalCheckoutSdk.Orders.OrderActionRequest());

        //    var response = await client.Execute(request);
        //    var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

        //    // ✅ Update order status in DB
        //    var payment = _service.GetPaymentByExternalOrderId(orderId);
        //    if (payment != null)
        //    {
        //        payment.Status = "Paid";
        //        _service.UpdatePayment(payment);
        //    }

        //    return Ok(result);
        //}

        [HttpPost("paypal/capture")]
        public IActionResult CapturePayPalPayment(string orderId)
        {
            var payment = _service.GetPaymentByExternalOrderId(orderId);
            if (payment != null)
            {
                payment.Status = "Paid";
                _service.UpdatePayment(payment);
            }

            // Return a fake PayPal response
            return Ok(new { id = orderId, status = "COMPLETED" });
        }



        [HttpPost("paypal/webhook")]
        public IActionResult PayPalWebhook([FromBody] PayPalWebhookDto payload)
        {
            var order = _service.GetPaymentById(payload.OrderId);
            if (payload.Status == "COMPLETED")
            {
                order.Status = "Paid";
            }
            else
            {
                order.Status = "Failed";
            }
            _service.UpdatePayment(order);
            return Ok();
        }


        [HttpPut("{id}")] public IActionResult Update(int id, Payment payment) { if (id != payment.Id) return BadRequest(); _service.UpdatePayment(payment); return NoContent(); }
        [HttpDelete("{id}")] public IActionResult Delete(int id) { _service.DeletePayment(id); return NoContent(); }

        [HttpGet("paypal/test")]
        public async Task<IActionResult> TestPayPal([FromServices] PayPalService paypalService)
        {
            var client = paypalService.GetClient();
            var request = new OrdersGetRequest("dummy-order-id");
            try
            {
                var response = await client.Execute(request);
                return Ok($"Status: {response.StatusCode}");
            }
            catch (HttpException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("status/{externalOrderId}")]
        public IActionResult GetPaymentStatus(string externalOrderId)
        {
            var payment = _service.GetPaymentByExternalOrderId(externalOrderId);
            if (payment == null)
                return NotFound(new { status = "Not Found" });

            return Ok(new
            {
                id = payment.Id,
                orderId = payment.OrderId,
                amount = payment.Amount,
                status = payment.Status,
                createdAt = payment.CreatedAt,
                externalOrderId = payment.ExternalOrderId
            });
        }


    }
}
