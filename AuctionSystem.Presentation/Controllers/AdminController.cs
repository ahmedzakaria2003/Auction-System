using Auction_System.Extensions;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Application.Specification;
using AuctionSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Auction_System.Controllers
{
    [Authorize(Roles = "Admin")]
    
    public class AdminController : ApiBaseController
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IServiceManager serviceManager, ILogger<AdminController> logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }


        [HttpGet("auctions-summary")]
        public async Task<ActionResult> GetAuctionsSummaryAsync()
        {
            try
            {
                _logger.LogInformation("Fetch auctions");
                var statistics = await _serviceManager.AdminService.GetAuctionStatisticsAsync();
                _logger.LogInformation("Auction statistics successfully retrieved.");

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving auction statistics: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");


            }

        }

        [HttpGet("seller-auctions-management")]
        public async Task<ActionResult> GetSellersAuctionsAsync([FromQuery] AuctionQueryParams specParams )
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                Unauthorized();
            }
            var isAdmin = User.IsInRole("Admin");

            var auctions = await _serviceManager.AdminService.GetSellersAuctions(specParams, userId!.Value , isAdmin);

            return Ok(auctions);

        }

        [HttpGet("all-users")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _serviceManager.AdminService.GetAllUsersAsync();
            return Ok(users);

        }
        [HttpPost("ban-user/{userId}")]
        public async Task<ActionResult> BanUserAsync(Guid userId)
        {
            await _serviceManager.AdminService.BanUserAsync(userId);
            return Ok("User banned successfully.");

        }
        [HttpPost("unban-user/{userId}")]
        public async Task<ActionResult> UnbanUserAsync(Guid userId)
        {
            await _serviceManager.AdminService.UnbanUserAsync(userId);
            return Ok("User unbanned successfully.");
        }
    }
}