using Auction_System.Controllers;
using Auction_System.Extensions;
using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AuctionController : ApiBaseController
{
    private readonly IServiceManager _serviceManager;

    public AuctionController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all-auctions")]
    public async Task<ActionResult<IEnumerable<AuctionListDto>>> GetAllAuctionsAsync()
    {
        var allAuctions = await _serviceManager.AuctionService.GetAllAuctionsAsync();
        return Ok(new { message = "All auctions retrieved successfully.", status = "success", data = allAuctions });
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpPost]
    public async Task<ActionResult> CreateAuctionAsync([FromForm] CreateAuctionDto dto)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var auction = await _serviceManager.AuctionService.CreateAuctionAsync(dto, userId.Value);
        return Ok(new { message = "Auction created successfully.", status = "success", id = auction });
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpPut("{auctionId:guid}")]
    public async Task<ActionResult> UpdateAuctionAsync([FromForm] UpdateAuctionDto dto, Guid auctionId)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var updatedAuction = await _serviceManager.AuctionService.UpdateAuctionAsync(auctionId, dto, userId.Value);
        return Ok(new { message = "Auction updated successfully.", status = "success", data = updatedAuction });
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpDelete("{auctionId:guid}")]
    public async Task<ActionResult> DeleteAuctionAsync(Guid auctionId)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var deletedAuction = await _serviceManager.AuctionService.DeleteAuctionAsync(auctionId, userId.Value);
        return Ok(new { message = "Auction deleted successfully.", status = "success", data = deletedAuction });
    }

    [AllowAnonymous]
    [HttpGet("active-auctions")]
    public async Task<ActionResult<IEnumerable<AuctionListDto>>> GetActiveAuctionsAsync([FromQuery] AuctionQueryParams queryParams)
    {
        var activeAuctions = await _serviceManager.AuctionService.GetActiveAuctionsAsync(queryParams);
        return Ok(new { message = "Active auctions retrieved successfully.", status = "success", data = activeAuctions });
    }

    [AllowAnonymous]
    [HttpGet("details/{auctionId:guid}")]
    public async Task<ActionResult> GetAuctionDetailsAsync(Guid auctionId)
    {
        var auctionDetails = await _serviceManager.AuctionService.GetAuctionDetailsAsync(auctionId);
        if (auctionDetails == null)
            return NotFound(new { message = "Auction not found.", status = "error" });

        return Ok(new { message = "Auction details retrieved successfully.", status = "success", data = auctionDetails });
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpGet("my-auctions")]
    public async Task<ActionResult<IEnumerable<AuctionListDto>>> GetAuctionsByCreatorAsync([FromQuery] AuctionQueryParams queryParams)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var auctionsByUser = await _serviceManager.AuctionService.GetAuctionsByCreatorAsync(queryParams, userId.Value);
        return Ok(new { message = "User's auctions retrieved successfully.", status = "success", data = auctionsByUser });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("canceled/{auctionId:guid}")]
    public async Task<ActionResult> CancelAuctionAsync(Guid auctionId)
    {
        var canceledAuction = await _serviceManager.AuctionService.CancelAuctionAsync(auctionId);
        return Ok(new { message = "Auction canceled successfully.", status = "success", data = canceledAuction });
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpPost("declare-winner/{auctionId}")]
    public async Task<ActionResult> DeclareWinner(Guid auctionId)
    {
        var result = await _serviceManager.AuctionService.DeclareWinnerAsync(auctionId);
        return Ok(new { message = "Winner declared successfully.", status = "success", data = result });
    }

    [AllowAnonymous]
    [HttpGet("hot-auctions")]
    public async Task<ActionResult<IEnumerable<AuctionListDto>>> GetHotAuctions([FromQuery] int take = 10)
    {
        var hotAuctions = await _serviceManager.AuctionService.GetHotAuctionsAsync(take);
        return Ok(new { message = "Hot auctions retrieved successfully.", status = "success", data = hotAuctions });
    }

    [Authorize(Roles = "Bidder")]
    [HttpGet("recommended-auctions")]
    public async Task<ActionResult<IEnumerable<AuctionListDto>>> GetRecommendedAuctionsForBidder()
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "User not authenticated." });

        var recommendedAuctions = await _serviceManager.AuctionService.GetRecommendedAuctionsForBidderAsync(userId.Value);
        return Ok(new { message = "Recommended auctions retrieved successfully.", status = "success", data = recommendedAuctions });
    }
}

