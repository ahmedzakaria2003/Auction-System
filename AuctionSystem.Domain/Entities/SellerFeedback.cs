using System;

namespace AuctionSystem.Domain.Entities
{
    public class SellerFeedback : BaseEntity
    {
       
        public Guid SellerId { get; set; }
        public Guid BidderId { get; set; }
        public Guid AuctionId { get; set; }
        public int Rating { get; set; } // ?? 1 ??? 5 ?????
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties 
        public ApplicationUser Seller { get; set; }
        public ApplicationUser Bidder { get; set; }
        public Auction Auction { get; set; }
    }
}