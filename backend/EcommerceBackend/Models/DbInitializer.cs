using EcommerceBackend.Models;
using System.Security.Cryptography;
using System.Text;

public static class DbInitializer
{
    public static void Seed(ShopDbContext context)
    {
        // Ensure DB is created
        context.Database.EnsureCreated();

        // Seed test user if not exists
        if (!context.Users.Any(u => u.Email == "test@example.com"))
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes("Password123"))
                );

                var user = new User
                {
                    Email = "test@example.com",
                    Username = "TestUser",
                    PasswordHash = hashedPassword
                };

                context.Users.Add(user);
            }
        }

        // Seed sample products
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Laptop", Price = 50000 },
                new Product { Name = "Smartphone", Price = 30000 },
                new Product { Name = "Shoes", Price = 2000 }
            );
        }

        context.SaveChanges();
    }
}
