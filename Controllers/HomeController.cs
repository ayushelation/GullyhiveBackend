using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GullyhiveBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("test-no-auth")]
        [AllowAnonymous]
        public IActionResult TestNoAuth()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            Console.WriteLine("=== TEST NO AUTH ===");
            Console.WriteLine($"Auth Header: '{authHeader}'");
            Console.WriteLine("=====================");

            return Ok(new
            {
                message = "This endpoint doesn't require auth",
                authHeader = authHeader,
                received = !string.IsNullOrEmpty(authHeader)
            });

        }

        [HttpGet("decode-tokens")]
        [AllowAnonymous]
        public IEnumerable<object> DecodeAndValidate()
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Bearer "))
                throw new Exception("Authorization header missing");

            var token = authHeader.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                ),

                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };

            var principal = handler.ValidateToken(token, parameters, out _);

            return principal.Claims.Select(c => new
            {
                c.Type,
                c.Value
            });
        }


        [HttpGet("decode-token")]
        [AllowAnonymous]
        public IActionResult DecodeToken(
            [FromHeader(Name = "Authorization")] string authHeader)
        {
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                return BadRequest("No Bearer token provided");

            var token = authHeader.Substring(7);

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var expiresUtc = jwtToken.ValidTo; //  CORRECT
                var nowUtc = DateTime.UtcNow;

                return Ok(new
                {
                    issuer = jwtToken.Issuer,
                    hasExp = expiresUtc != DateTime.MinValue,
                    expiresUtc,
                    nowUtc,
                    isExpired = expiresUtc < nowUtc,
                    claims = jwtToken.Claims.Select(c => new { c.Type, c.Value })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        // Any authenticated user
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var name = User.FindFirstValue(ClaimTypes.Name);

            return Ok(new
            {
                message = "Welcome",
                userId,
                role,
                name
            });
        }

        // Admin only
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return Ok(new { message = "Admin dashboard access granted" });
        }

        //Buyer only
        [HttpGet("buyer")]
        [Authorize(Roles = "Buyer")]
        public IActionResult BuyerDashboard()
        {
            return Ok("Buyer dashboard access granted");
        }

        // Seller only
        [HttpGet("seller")]
        [Authorize(Roles = "Seller")]
        public IActionResult SellerDashboard()
        {
            return Ok("Seller dashboard access granted");
        }

        // Admin + SuperAdmin
        [HttpGet("admin-super")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult AdminSuperAdmin()
        {
            return Ok("Admin & SuperAdmin access granted");
        }

    }
}
