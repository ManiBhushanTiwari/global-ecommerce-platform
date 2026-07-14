using System;
using System.Collections.Generic;

namespace EcommerceBackend.Models;

public partial class Shipping
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? TrackingNumber { get; set; }

    public string? Carrier { get; set; }

    public DateTime ShippedDate { get; set; }

    public string? Status { get; set; }
}
