using Auction_System.Extensions;
using AuctionSystem.Application.DTOS.ProfileDTO;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Application.Services.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auction_System.Controllers
{
    [Authorize(Roles ="Bidder")]
  
    public class ProfileController : ApiBaseController
    {
        private readonly IServiceManager _serviceManager;

        public ProfileController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;

        }

        [HttpGet("active-bids")]
        public async Task<ActionResult<IEnumerable<ActiveBiddingDto>>> GetActiveBids()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authorized or user ID is missing" });
            var auctions = await _serviceManager.ProfileService.GetActiveBiddingAsync(userId.Value);
            return Ok(auctions);

        }

        [HttpGet("won-auctions")]
        public async Task<ActionResult<IEnumerable<AuctionWonDto>>> GetWonAuctions()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authorized or user ID is missing" });

            var Wonauctions = await _serviceManager.ProfileService.GetWonAuctionAsync(userId.Value);
            return Ok(Wonauctions);

        }

    }
}
