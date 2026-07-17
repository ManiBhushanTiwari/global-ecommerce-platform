using EcommerceBackend.Models;
using EcommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EcommerceBackend.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly ShopDbContext _context; // your EF Core DbContext

        private readonly IConfiguration _configuration;

        public AuthController(ShopDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //public AuthController(TokenService tokenService, ShopDbContext context)
        //{
        //    _tokenService = tokenService;
        //    _context = context;
        //}

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



        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginDto loginDto)
        //{
        //    // Validate user against DB
        //    var user = _context.Users.SingleOrDefault(u => u.Email == loginDto.Email);
        //    if (user == null)
        //        return Unauthorized("Invalid credentials");

        //    // Verify password hash (basic comparison - in production use proper hashing like BCrypt)
        //    if (!VerifyPassword(loginDto.Password, user.PasswordHash))
        //        return Unauthorized("Invalid credentials");

        //    // Generate JWT
        //    var token = _tokenService.GenerateToken(user);

        //    // Return token + userId
        //    return Ok(new { token, userId = user.Id });
        //}


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (_configuration.GetValue<bool>("DemoMode"))
            {
                // Always return a valid user, no matter what was entered
                var demoUser = _context.Users.FirstOrDefault()
                               ?? new User { Username = "Demo", Email = "demo@example.com" };

                return Ok(new { Token = GenerateJwtToken(demoUser), User = demoUser });
            }

            // Normal login flow (hash + compare)
            using (var sha256 = SHA256.Create())
            {
                string hashedInput = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password))
                );

                var user = _context.Users
                    .FirstOrDefault(u => u.Email == loginDto.Email && u.PasswordHash == hashedInput);

                if (user == null) return Unauthorized("Invalid credentials");

                return Ok(new { Token = GenerateJwtToken(user), User = user });
            }
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

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("username", user.Username),
        new Claim("email", user.Email)
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }


}
