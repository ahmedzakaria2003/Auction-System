using Auction_System.Extensions;
using AuctionSystem.Application.DTOS.AdminDTO;
using AuctionSystem.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction_System.Controllers
{
    [Authorize (Roles ="Seller")]
   
    public class SellerController : ApiBaseController
    {
        private readonly IServiceManager _serviceManager;

        public SellerController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        [HttpGet("auctions-summary")]
        public async Task<ActionResult<AuctionStatisticsDto>> GetAuctionsSummaryAsync()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authorized or user ID is missing" });

            var statistics = await _serviceManager.SellerService.GetSellerStatistics(userId!.Value);

            return Ok(statistics);

        }
    }
}
