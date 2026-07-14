using EcommerceBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Orders;

namespace EcommerceBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public AnalyticsController(ShopDbContext context)
        {
            _context = context;
        }

        [HttpGet("orders-per-day")]
        public IActionResult GetOrdersPerDay()
        {
            var data = _context.Orders
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new { date = g.Key, count = g.Count() })
                .ToList();
            return Ok(data);
        }

        [HttpGet("conversion-rates")]
        public IActionResult GetConversionRates()
        {
            var total = _context.Orders.Count();
            var completed = _context.Orders.Count(o => o.Status == "Completed");
            var rate = total == 0 ? 0 : (double)completed / total * 100;
            return Ok(new[] { new { metric = "ConversionRate", value = rate } });
        }

        [HttpGet("failed-payments")]
        public IActionResult GetFailedPayments()
        {
            var failed = _context.Payments.Count(p => p.Status == "Failed");
            return Ok(failed);
        }
    }

}
