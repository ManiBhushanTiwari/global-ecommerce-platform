using EcommerceBackend.Models;
using EcommerceBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly ShippingService _service;
        public ShippingController(ShippingService service) => _service = service;

        [HttpGet] public IActionResult GetAll() => Ok(_service.GetAllShippings());
        [HttpGet("{id}")] public IActionResult GetById(int id) => _service.GetShippingById(id) is Shipping s ? Ok(s) : NotFound();
        [HttpPost] public IActionResult Create(Shipping shipping) { _service.AddShipping(shipping); return CreatedAtAction(nameof(GetById), new { id = shipping.Id }, shipping); }
        [HttpPut("{id}")] public IActionResult Update(int id, Shipping shipping) { if (id != shipping.Id) return BadRequest(); _service.UpdateShipping(shipping); return NoContent(); }
        [HttpDelete("{id}")] public IActionResult Delete(int id) { _service.DeleteShipping(id); return NoContent(); }
        [HttpGet("tracking/{orderId}")]
        public IActionResult GetTracking(int orderId)
        {
            var shipping = _service.GetShippingById(orderId);
            if (shipping == null)
                return NotFound(new { message = "No tracking info found" });

            return Ok(new
            {
                orderId = shipping.OrderId,
                carrier = shipping.Carrier,
                trackingNumber = shipping.TrackingNumber,
                status = shipping.Status
            });
        }

    }
}
