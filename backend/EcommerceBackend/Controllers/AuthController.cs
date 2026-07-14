using EcommerceBackend.Models;
using EcommerceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EcommerceBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly ShopDbContext _context; // your EF Core DbContext

        public AuthController(TokenService tokenService, ShopDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // Check if email already exists
            if (_context.Users.Any(u => u.Email == registerDto.Email))
            {
                return BadRequest("Email already registered");
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password))
                );

                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = hashedPassword
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return Ok("User registered successfully");
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Validate user against DB
            var user = _context.Users.SingleOrDefault(u => u.Email == loginDto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            // Verify password hash (basic comparison - in production use proper hashing like BCrypt)
            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            // Generate JWT
            var token = _tokenService.GenerateToken(user);

            // Return token + userId
            return Ok(new { token, userId = user.Id });
        }

        //private bool VerifyPassword(string password, string hash)
        //{
        //    // Simple string comparison - replace with proper BCrypt or PBKDF2 in production
        //    // For now, assume hash is SHA256 of password
        //    using (var sha256 = SHA256.Create())
        //    {
        //        var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        //        return hashedPassword == hash;
        //    }
        //}

        private bool VerifyPassword(string password, string hash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
                return hashedPassword == hash;
            }
        }


    }


}
