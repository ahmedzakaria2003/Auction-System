using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Application.Contracts
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        Task<IEnumerable<Bid>> GetBidsForAuctionAsync(Guid auctionId);
        Task<Bid?> GetHighestBidForAuctionAsync(Guid auctionId);
        Task<bool> HasUserBidAsync(Guid auctionId, Guid userId);




    }
}