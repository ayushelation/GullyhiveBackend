using GullyHive.Auth.Models;
using GullyHive.Auth.Services;
using GullyHive.Seller.Models;
using GullyHive.Seller.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using StackExchange.Redis;

namespace GullyHive.Seller.Controllers
{
    [ApiController]
    [Route("api/seller")]
    [Authorize(Roles = "Seller")]
    public class SellerController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly IDatabase _redis;
        private readonly IUserService _userService;
        private readonly IDashboardService _dashboardService;
        private readonly ILeadService _ileadService;
        private readonly IPublicProfileService _service;
        private readonly IResponseService _responseService;
        private readonly IHelpService _helpService;
        private readonly IReferralService _referralService;
        private readonly IPartnerEarningService _earningService;
  
        public SellerController(IConfiguration config, IConnectionMultiplexer redis, IUserService userService, IDashboardService dashboardService, ILeadService ileadService, IPublicProfileService service, IResponseService responseService,
            IHelpService helpService,
            IReferralService referralService,
            IPartnerEarningService earningService
            )
        {
            _config = config;
            _connectionString = config.GetConnectionString("ConStr")!;
            _redis = redis.GetDatabase();
            _userService = userService;
            _dashboardService = dashboardService;
            _ileadService = ileadService;
            _service = service;
            _responseService = responseService;
            _helpService = helpService;
            _referralService = referralService;
            _earningService = earningService;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok("Seller module alive");
        }

        //[Authorize] // Require a valid JWT token
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            var data = await _dashboardService.GetDashboardDataAsync(User.Identity!.Name!); // Pass seller email/username
            return Ok(new { success = true, data });
        }
        [HttpGet("leads")]
        public async Task<IActionResult> GetLeads()
        {
            var leads = await _ileadService.GetRecentLeadsAsync();
            return Ok(new { success = true, data = leads });
        }

        [HttpGet("completeProfile/{sellerId}")]
        [Authorize]
        public async Task<IActionResult> GetPublicProfile(long sellerId)
        {
            var profile = await _service.GetPublicProfileAsync(sellerId);

            if (profile == null)
                return NotFound(new { success = false, message = "Seller not found" });

            return Ok(new { success = true, data = profile });
        }
        [HttpPut("editprofile/{providerId}")]
        public async Task<IActionResult> UpdateProfile(long providerId, [FromBody] UpdateProfileDto dto)
        {
            var success = await _service.UpdateProfileAsync(providerId, dto);
            if (!success)
                return BadRequest("Failed to update profile");

            return NoContent();
        }


        // =========================
        // RESPONSES (CRUD) inside SellerController
        // =========================

        // GET: /api/seller/responses





        // GET all responses for a seller (frontend passes sellerId)
        [HttpGet("responses/seller/{sellerId}")]
        public async Task<IActionResult> GetResponsesBySeller(long sellerId)
        {
            var responses = await _responseService.GetMyResponses(sellerId);
            return Ok(new { success = true, data = responses });
        }

        // GET single response
        [HttpGet("responses/{id}")]
        public async Task<IActionResult> GetResponse(long id)
        {
            var sellerId = long.Parse(User.FindFirst("user_id")!.Value);
            var response = await _responseService.GetResponse(id, sellerId);
            if (response == null)
                return NotFound(new { success = false, message = "Response not found" });

            return Ok(new { success = true, data = response });
        }

        // POST response
        [HttpPost("responses")]
        public async Task<IActionResult> CreateResponse([FromBody] CreateResponseDto dto)
        {
            var sellerId = long.Parse(User.FindFirst("user_id")!.Value);
            var id = await _responseService.Create(sellerId, dto);
            return Ok(new { success = true, id });
        }

        // PUT response
        [HttpPut("responses/{id}")]
        public async Task<IActionResult> UpdateResponse(long id, [FromBody] UpdateResponseDto dto)
        {
            var sellerId = long.Parse(User.FindFirst("user_id")!.Value);
            var success = await _responseService.Update(id, sellerId, dto);
            return Ok(new { success });
        }

        // PATCH response status
        [HttpPatch("responses/{id}/status")]
        public async Task<IActionResult> UpdateResponseStatus(long id, [FromQuery] string status)
        {
            var sellerId = long.Parse(User.FindFirst("user_id")!.Value);
            var success = await _responseService.UpdateStatus(id, sellerId, status);
            return Ok(new { success });
        }

        // DELETE response
        [HttpDelete("responses/{id}")]
        public async Task<IActionResult> DeleteResponse(long id)
        {
            var sellerId = long.Parse(User.FindFirst("user_id")!.Value);
            var success = await _responseService.Delete(id, sellerId);
            return Ok(new { success });
        }
    
            [HttpGet("faqs")]
        public async Task<IActionResult> GetHelp()
        {
            var (categories, faqs) = await _helpService.GetHelpDataAsync();

            return Ok(new
            {
                success = true,
                categories,
                faqs
            });
        }

            [HttpGet("refer/{sellerId}")]
            public async Task<IActionResult> GetReferrals(int sellerId)
            {
                var data = await _referralService.GetReferralsAsync(sellerId);
                return Ok(new { success = true, data });
            }

          //  [HttpGet("partner_earnings/user/{userId}")]
            //public async Task<IActionResult> GetEarnings(int userId)
            //{
            //    var data = await _earningService.GetEarningsAsync(userId);
            //    return Ok(new { success = true, data });
            //}

            //[HttpGet("partner_earnings/user/{userId}/total")]
            //public async Task<IActionResult> GetTotalEarnings(int userId)
            //{
            //    var total = await _earningService.GetTotalEarningsAsync(userId);
            //    return Ok(new { success = true, totalEarnings = total });
            //}
        
    }

}



