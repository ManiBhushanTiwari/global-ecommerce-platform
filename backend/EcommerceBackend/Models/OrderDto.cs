namespace EcommerceBackend.Models
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public List<OrderItemDto>? Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        // Snapshot fields
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
