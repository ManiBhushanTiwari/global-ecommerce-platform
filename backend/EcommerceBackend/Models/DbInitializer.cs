using EcommerceBackend.Models;
using System.Security.Cryptography;
using System.Text;

public static class DbInitializer
{
    public static void Seed(ShopDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // ✅ Users
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { Username = "Admin", Email = "admin@example.com", PasswordHash = "Admin123", Address = "HQ Address", CreatedAt = DateTime.Now },
                new User { Username = "TestUser", Email = "test@example.com", PasswordHash = "Password123", Address = "Noida, India", CreatedAt = DateTime.Now }
            );
            context.SaveChanges();
        }

        // ✅ Products
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Laptop", Category = "Electronics", Price = 75000, Stock = 10 },
                new Product { Name = "Smartphone", Category = "Electronics", Price = 30000, Stock = 25 },
                new Product { Name = "Shoes", Category = "Fashion", Price = 2500, Stock = 50 }
            );
            context.SaveChanges();
        }

        // ✅ Orders
        if (!context.Orders.Any())
        {
            var user = context.Users.First();
            var order = new Order
            {
                UserId = user.Id,
                Status = "Completed",
                TotalAmount = 77500,
                CreatedAt = DateTime.Now,
                ShippingAddress = "Noida, India",
                BillingAddress = "Noida, India",
                CustomerName = user.Username,
                CustomerEmail = user.Email,
                ExternalOrderId = Guid.NewGuid().ToString(),
                Provider = "Internal"
            };
            context.Orders.Add(order);
            context.SaveChanges();

            // ✅ OrderItems
            var laptop = context.Products.First(p => p.Name == "Laptop");
            var shoes = context.Products.First(p => p.Name == "Shoes");

            context.OrderItems.AddRange(
                new OrderItem { OrderId = order.Id, ProductId = laptop.Id, Quantity = 1, Price = laptop.Price },
                new OrderItem { OrderId = order.Id, ProductId = shoes.Id, Quantity = 2, Price = shoes.Price }
            );
            context.SaveChanges();

            // ✅ Payment
            context.Payments.Add(new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                Status = "Paid",
                PaymentDate = DateTime.Now,
                Provider = "PayPal",
                ExternalOrderId = order.ExternalOrderId,
                CreatedAt = DateTime.Now
            });
            context.SaveChanges();

            // ✅ Shipping
            context.Shippings.Add(new Shipping
            {
                OrderId = order.Id,
                TrackingNumber = "TRACK123456",
                Carrier = "DHL",
                ShippedDate = DateTime.Now,
                Status = "Delivered"
            });
            context.SaveChanges();
        }
    }
}

