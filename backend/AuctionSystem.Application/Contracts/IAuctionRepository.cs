using AuctionSystem.Application.DTOS.ProfileDTO;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;

namespace AuctionSystem.Application.Contracts
{
    public interface IAuctionRepository : IGenericRepository<Auction>
    {
        Task<IEnumerable<Auction>> GetAllAuctionsAsync();
        Task<IEnumerable<Auction>> GetAuctionsByCreatorIdAsync( Guid userId);
        Task<Auction?> GetAuctionWithDetailsAsync(Guid auctionId);
        Task<Auction?> GetAuctionWithImagesAsync(Guid auctionId);
        Task<IEnumerable<Auction>> GetAuctionsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Auction>> GetWonAuctionAsync(Guid userId);
        Task<IEnumerable<Auction>> GetActiveBiddingAsync(Guid userId);
        Task<Auction?> GetByPaymentIntentIdAsync(string paymentIntentId);
        Task<IEnumerable<Auction>> GetAuctionsThatEndedWithoutWinnerAsync();
        Task<IEnumerable<Auction>> GetRecommendedAuctionsForBidderAsync(Guid bidderId);
        Task<IEnumerable<Auction>> GetHotAuctionsAsync(int take = 10);


    }

}