using AuctionSystem.Application.DTOS.SellerFeedbackDTO;
using AuctionSystem.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionSystem.API.Controllers
{
   
    public class SellerFeedbackController : ApiBaseController
    {
        private readonly ISellerFeedbackService _feedbackService;

        public SellerFeedbackController(ISellerFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [Authorize] // يمكن تخصيص الصلاحيات حسب الحاجة
        public async Task<IActionResult> AddFeedback([FromBody] SellerFeedbackDto feedbackDto)
        {
            await _feedbackService.AddFeedbackAsync(feedbackDto);
            return Ok(new { message = "Feedback added successfully." });
        }

        [HttpGet("seller/{sellerId:guid}")]
        public async Task<IActionResult> GetSellerFeedbacks(Guid sellerId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksForSellerAsync(sellerId);
            return Ok(feedbacks);
        }

        [HttpGet("seller/{sellerId:guid}/average-rating")]
        public async Task<IActionResult> GetSellerAverageRating(Guid sellerId)
        {
            var avg = await _feedbackService.GetAverageRatingForSellerAsync(sellerId);
            return Ok(new { averageRating = avg });
        }
    }
}