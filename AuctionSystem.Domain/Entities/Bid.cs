namespace AuctionSystem.Domain.Entities
{
    public class Bid : BaseEntity
    {

      
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; } = default!; // Navigation property
        public Guid BidderId { get; set; }
        public ApplicationUser Bidder { get; set; } = default!; // Navigation property


    }
}