using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Application.DTOS.SellerFeedbackDTO
{
    public class SellerFeedbackDto
    {
        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public Guid BidderId { get; set; }

        [Required]
        public Guid AuctionId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment can't be longer than 1000 characters.")]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}