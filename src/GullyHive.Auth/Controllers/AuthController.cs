using GullyHive.Auth.Models;
using GullyHive.Auth.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GullyHive.Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly IDatabase _redis;
        private readonly IUserService _userService;
        private readonly IServiceCategoryService _service;


        public AuthController(IConfiguration config, IConnectionMultiplexer redis, IUserService userService, IServiceCategoryService service)
        {
            _config = config;
            _connectionString = config.GetConnectionString("ConStr")!;
            _redis = redis.GetDatabase();
            _userService = userService;
            _service = service;
        }

        // =======================================
        // Test Module
        // =======================================
        [HttpGet("ping")]
        public IActionResult Ping()
        => Ok("Auth module alive");


        // =====================================================
        // Generate JWT Tokens
        // =====================================================
        //    [NonAction]
        //    public string GenerateToken(string userId, string role)
        //    {
        //        var claims = new List<Claim>
        //{
        //    new Claim(JwtRegisteredClaimNames.Sub, userId),
        //    new Claim("role", role),  // Role Claim in JWT
        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //};

        //        var key = new SymmetricSecurityKey(
        //            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        //        );

        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var token = new JwtSecurityToken(
        //            issuer: _config["Jwt:Issuer"],
        //            audience: _config["Jwt:Audience"],
        //            claims: claims,
        //            expires: DateTime.UtcNow.AddMinutes(
        //                _config.GetValue<int>("Jwt:ExpireMinutes")
        //            ),
        //            signingCredentials: creds
        //        );

        //        return new JwtSecurityTokenHandler().WriteToken(token);
        //    }

        [NonAction]
        public string GenerateToken(string userId, string role, string displayName)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, userId),  // user ID
        new Claim(ClaimTypes.Name, displayName),        // <-- Add this
        new Claim("role", role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    _config.GetValue<int>("Jwt:ExpireMinutes")
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        // ============================
        // LOGIN
        // ============================
        [HttpPost("login")]
        public IActionResult Login([FromBody] Models.LoginRequest request)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"
            SELECT
                u.id,
                u.email,
                u.phone,
                u.display_name,
                u.password_hash,
                r.name AS role
            FROM users u
            JOIN user_roles ur ON ur.user_id = u.id
            JOIN roles r ON r.id = ur.role_id
            WHERE lower(u.email) = lower(@u)
               OR u.phone = @u
            LIMIT 1;
            ", conn);

            cmd.Parameters.AddWithValue("@u", request.Username.Trim());

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return Unauthorized("Invalid credentials");

            var passwordHash = reader.GetString(
                reader.GetOrdinal("password_hash")
            );

            if (!BCrypt.Net.BCrypt.Verify(request.Password, passwordHash))
                return Unauthorized("Invalid credentials");

            var userId = reader.GetInt64(reader.GetOrdinal("id")).ToString();
            var role = reader.GetString(reader.GetOrdinal("role"));

            var displayName =
                reader["display_name"]?.ToString()
                ?? reader["email"]?.ToString()
                ?? reader["phone"]!.ToString();

            var token = GenerateToken(userId, role,displayName);

            return Ok(new
            {
                token,
                role,
                name = displayName
            });
        }
        // ======================================
        // LOGOUT
        // ======================================

        [HttpPost("logout")]  
        public IActionResult Logout()
        {
            // JWT is stateless → nothing to invalidate on server
            // Client must delete token
            return Ok(new { message = "Logged out successfully" });
        }



        // ============================
        // REGISTER
        //// ============================
        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] Models.RegisterRequest request)

        [HttpGet("parents")]
        public async Task<IActionResult> GetParents()
                    => Ok(await _service.GetServicesAsync());

        [HttpGet("{parentId}/children")]
        public async Task<IActionResult> GetChildren(long parentId)
            => Ok(await _service.GetCategoriesAsync(parentId));



        [HttpPost("register")]  
        public async Task<IActionResult> Register([FromForm] Models.RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.RegisterUserAsync(model);
            return Ok(new { message = "Registration successful" });
        }
    }

}
