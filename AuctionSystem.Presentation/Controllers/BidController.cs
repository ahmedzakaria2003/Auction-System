using Auction_System.Extensions;
using AuctionSystem.Application.DTOS.BidDTO;
using AuctionSystem.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auction_System.Controllers
{
    [Authorize(Roles ="Bidder")]
  
    public class BidController : ApiBaseController
    {
        private readonly IServiceManager _serviceManager;
        public BidController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("place-bid")]
        public async Task<ActionResult> AddBidAsync([FromBody] AddBidDto bidDto)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized();
            var result = await _serviceManager.BidService.AddBidAsync(bidDto , userId.Value  );

            return Ok(new { message = "Bid placed successfully.", bidId = result });
        }

        [HttpGet("history/{auctionId}")]
        public async Task<ActionResult<IEnumerable<BidDto>>> GetBidsByAuctionIdAsync(Guid auctionId)
        {
            var result = await _serviceManager.BidService.GetBidsForAuctionAsync(auctionId);
            return Ok(result);
        }

        [HttpGet("highest/{auctionId}")]
        public async Task<ActionResult> GetHighestBidForAuctionAsync (Guid auctionId)
        {
            var result = await _serviceManager.BidService.GetHighestBidForAuctionAsync(auctionId);
            return Ok(result);
        }



    }
}
