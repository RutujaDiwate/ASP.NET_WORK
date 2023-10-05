using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HRMS_APP.Folder.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration for reading app settings
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Registration endpoint
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationModel model)
        {
            try
            {
                // Validate the model (you can add more validation as needed)
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Email and password are required.");
                }

                // Here, you would typically store the user's registration data in your database.
                // For simplicity, we'll just return a success message.
                return Ok("Registration successful!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Login endpoint
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                // Validate the model (you can add more validation as needed)
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Email and password are required.");
                }

                // Here, you would typically validate the user's credentials against your database.
                // For simplicity, we'll assume a sample user.
                if (model.Email == "sample@example.com" && model.Password == "password123")
                {
                    // Create a JWT token
                    var token = GenerateJwtToken(model.Email);

                    // Return the token to the client
                    return Ok(new { token });
                }
                else
                {
                    // Return an unauthorized error
                    return Unauthorized("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Generate a JWT token for a user
        private string GenerateJwtToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(1), // Set token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
