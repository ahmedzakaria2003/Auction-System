namespace AuctionSystem.Domain.Entities
{
    public class Category : BaseEntity
    {
    
        public string Name { get; set; } = default!;

        // Navigation
        public ICollection<Auction> Auctions { get; set; } = [];


    }
}