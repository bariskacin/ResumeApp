using Microsoft.AspNetCore.Mvc;
using ResumeApp.server.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace ResumeApp.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ResumeDbContext _context;

        public AuthController(ResumeDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // Check if user already exists
            var existingUser = await _context.People.FirstOrDefaultAsync(p => p.Email == model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            // Hash password
            var passwordHash = HashPassword(model.Password);

            // Create new person
            var person = new Person
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Email == model.Email);
            if (person == null || !VerifyPassword(model.Password, person.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // In a real app, you would generate a JWT token here
            return Ok(new { message = "Login successful", userId = person.Id });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }

    public class RegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
