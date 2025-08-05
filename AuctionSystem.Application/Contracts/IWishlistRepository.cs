using AuctionSystem.Infrastructure.RedisModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Contracts
{
    public interface IWishlistRepository
    {
        Task<BidderWishlist?> AddToWishlistAsync( BidderWishlist wishlist, TimeSpan ?ttl =null);
        Task<BidderWishlist?> RemoveFromWishlistAsync(string key, Guid auctionId);

        Task<BidderWishlist?> GetBidderWishlistAsync(string userId);


    }
}
