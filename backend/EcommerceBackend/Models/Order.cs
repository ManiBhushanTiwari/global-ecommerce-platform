using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceBackend.Models;

public partial class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Snapshot of checkout info
    public string? ShippingAddress { get; set; }
    public string? BillingAddress { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }

    // Payment provider references
    public string? ExternalOrderId { get; set; }
    public string? Provider { get; set; }

    // Navigation
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual User User { get; set; } = null!;
}


