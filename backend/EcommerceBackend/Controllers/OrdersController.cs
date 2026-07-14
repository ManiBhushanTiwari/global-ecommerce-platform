using EcommerceBackend.Models;
using EcommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;
        public OrdersController(OrderService service) => _service = service;

        //[Authorize]
        [HttpGet] public IActionResult GetAll() => Ok(_service.GetAllOrders());
        [HttpGet("{id}")] public IActionResult GetById(int id) => _service.GetOrderById(id) is Order o ? Ok(o) : NotFound();
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] OrderDto orderDto)
        {
            if (orderDto == null) return BadRequest("Order is null");
            var user = _service.Users.Find(orderDto.UserId);
            if (user == null) return BadRequest("User not found");
            var order = new Order
            {
                UserId = orderDto.UserId,
                TotalAmount = orderDto.TotalAmount,
                Status = orderDto.Status,
                ShippingAddress = orderDto.ShippingAddress,
                BillingAddress = orderDto.BillingAddress,
                CustomerName = orderDto.CustomerName,
                CustomerEmail = orderDto.CustomerEmail,
                OrderItems = orderDto.Items?.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList() ?? new List<OrderItem>()
            };

            _service.AddOrder(order);

            return Ok(new { id = order.Id, totalAmount = order.TotalAmount });
        }


        //[HttpPost("create-multiple")] // multiple orders
        //public IActionResult CreateMultiple([FromBody] List<OrderDto> orderDtos)
        //{
        //    if (orderDtos == null || !orderDtos.Any())
        //        return BadRequest("Orders are null or empty");

        //    foreach (var orderDto in orderDtos)
        //    {
        //        var order = new Order
        //        {
        //            UserId = orderDto.UserId,
        //            TotalAmount = orderDto.TotalAmount,
        //            Status = orderDto.Status,
        //            OrderItems = orderDto.Items?.Select(i => new OrderItem
        //            {
        //                ProductId = i.ProductId,
        //                Quantity = i.Quantity
        //            }).ToList() ?? new List<OrderItem>()
        //        };

        //        _service.AddOrder(order);
        //    }

        //    return Ok(new { message = "Orders placed successfully", count = orderDtos.Count });
        //}
        [HttpGet("user/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            var orders = _service.GetOrdersByUser(userId);
            if (orders == null || !orders.Any())
                return NotFound(new { message = "No orders found for this user" });

            return Ok(orders);
        }

        [HttpPut("{id}")] public IActionResult Update(int id, Order order) { if (id != order.Id) return BadRequest(); _service.UpdateOrder(order); return NoContent(); }
        [HttpDelete("{id}")] public IActionResult Delete(int id) { _service.DeleteOrder(id); return NoContent(); }
    }
}
