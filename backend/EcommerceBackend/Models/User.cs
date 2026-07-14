using System;
using System.Collections.Generic;

namespace EcommerceBackend.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
    public string? Address { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
