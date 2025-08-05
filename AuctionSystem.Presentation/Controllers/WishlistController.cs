using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction_System.Controllers
{
    [Authorize(Roles = "Bidder")]

    public class WishlistController : ApiBaseController
    {
        private readonly IServiceManager _serviceManager;
        public WishlistController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }



        [HttpGet("{key}")]
        public async Task<ActionResult> GetWishlist(string key)
        {
            var wishlist = await _serviceManager.WishlistService.GetWishlistAsync(key);
            return Ok(wishlist);
        }

        [HttpPost("{key}")]
        public async Task<ActionResult> AddToWishlist(string key, [FromBody] AuctionListDto dto)
        {
            var updated = await _serviceManager.WishlistService.AddToWishlistAsync(dto, key);
            return Ok(new { message = "Item added to wishlist successfully", data = updated });
        }

        [HttpDelete("{key}/{auctionId}")]
        public async Task<ActionResult> RemoveFromWishlist(string key, Guid auctionId)
        {
            var updated = await _serviceManager.WishlistService.RemoveFromWishlistAsync(key, auctionId);
            return Ok(new { message = "Item removed from wishlist successfully", data = updated });
        }

    }
}
