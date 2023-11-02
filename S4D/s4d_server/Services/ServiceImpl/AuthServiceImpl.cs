using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using s4dServer.DTOs.Request;
using s4dServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace s4dServer.Services.ServiceImpl
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly S4DContext _context;
       

        public AuthServiceImpl(IConfiguration configuration, S4DContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        private string HashPassword(string password)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_configuration["HashPwd:Secret"] ?? ""));
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(passwordHash);
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim("UserId", user.UserID.ToString()),
            new Claim("Username", user.Username ?? ""),
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"] ?? ""));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            claims.Add(new Claim("iat_utc", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));
            claims.Add(new Claim("exp_utc", DateTimeOffset.UtcNow.AddHours(_configuration.GetValue<int>("token_lifetime")).ToUnixTimeSeconds().ToString()));

            var token = new JwtSecurityToken(
                _configuration["JwtConfig:Issuer"],
                _configuration["JwtConfig:Audience"],
                claims,
                expires: DateTime.Now.AddHours(_configuration.GetValue<int>("token_lifetime")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ActionResult> Login(LoginRequestDTO model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Status == 1 && u.Username.ToLower() == model.Username);

            if (user == null || user.Password != HashPassword(model.Password))
            {
                return new BadRequestObjectResult(new { message = "Wrong username or password" });
            }

            var token = GenerateJwtToken(user);
            return new OkObjectResult(new { token });
        }

        public async Task<ActionResult> Register(RegisterRequestDTO model)
        {
            // Check if the username is already taken
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == model.Username.ToLower());
            if (existingUser != null)
            {
                return new BadRequestObjectResult(new { message = "Username is already taken" });
            }

            // Check if the email is already registered
            var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
            if (existingEmail != null)
            {
                return new BadRequestObjectResult(new { message = "Email is already registered" });
            }

            // Create a new User object
            var newUser = new User
            {
                Username = model.Username,
                Password = HashPassword(model.Password),
                Email = model.Email,
                Role = "User", // Set the default role for new users
                Status = 1 // Set the default status for new users
            };

            // Save the new user to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Generate JWT token for the registered user
            var token = GenerateJwtToken(newUser);

            return new OkObjectResult(new { token });
        }
    }
}
