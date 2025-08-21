using Auction_System.Extensions;
using AuctionSystem.Application.DTOS.SellerFeedbackDTO;
using AuctionSystem.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction_System.Controllers
{
    [Authorize(Roles = "Bidder")]

    public class SellerFeedbackController : ApiBaseController
    {

        private readonly IServiceManager _serviceManager;

        public SellerFeedbackController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;

        }

        [HttpPost]
        public async Task<ActionResult> AddFeedback([FromBody] AddFeedbackRequest request)
        {
            var bidderId = User.GetUserId();
            if (bidderId == null)
                return Unauthorized(new { message = "User not authorized or user ID is missing" });

            var feedbackDto = new SellerFeedbackDto
            {
                AuctionId = request.AuctionId,
                BidderId = bidderId.Value,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _serviceManager.SellerFeedbackService.AddFeedbackAsync(feedbackDto);
            return Ok(new
            {
                message = "Feedback add successfully.",
            });
        }

        [HttpGet("seller/{sellerId:guid}")]
        public async Task<ActionResult<List<SellerFeedbackDto>>> GetSellerFeedbacks(Guid sellerId)
        {
            var feedbacks = await _serviceManager.SellerFeedbackService.GetFeedbacksForSellerAsync(sellerId);
            return Ok(feedbacks);
        }

        [HttpGet("seller/{sellerId:guid}/average-rating")]
        public async Task<ActionResult> GetSellerAverageRating(Guid sellerId)
        {
            var avg = await _serviceManager.SellerFeedbackService.GetAverageRatingForSellerAsync(sellerId);
            return Ok(new { averageRating = avg });
        }

        [HttpGet("auction/{auctionId:guid}/has-rated")]
        public async Task<ActionResult> HasUserRatedAuction(Guid auctionId)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authorized or user ID is missing" });

            var hasRated = await _serviceManager.SellerFeedbackService.HasUserRatedAuctionAsync(userId.Value, auctionId);

            return Ok(new { hasRated });
        }
    }
    }
