using AuctionSystem.Application.Contracts;
using AuctionSystem.Infrastructure.RedisModels;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Repositories
{
   public class WishlistRepository : IWishlistRepository
    {
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions 
           _serializerOptions = new() { PropertyNameCaseInsensitive = true };

        public WishlistRepository(IConnectionMultiplexer connection)
        {
          
            _database = connection.GetDatabase();

        }


        public async Task<BidderWishlist?> GetBidderWishlistAsync(string userId)
        {
            var data = await _database.StringGetAsync(userId);

            return data.IsNullOrEmpty ? null :
                     JsonSerializer.Deserialize<BidderWishlist>(data!, _serializerOptions);
        }
        public async Task<BidderWishlist?> AddToWishlistAsync(BidderWishlist wishlist, TimeSpan? ttl = null)
        {
            var data = JsonSerializer.Serialize(wishlist);
            await _database.StringSetAsync(wishlist.Id, data, ttl ?? TimeSpan.FromDays(1));
            return await GetBidderWishlistAsync(wishlist.Id);
        }

     

        public async Task<BidderWishlist?> RemoveFromWishlistAsync(string key, Guid auctionId)
        {
            var wishlist = await GetBidderWishlistAsync(key);
            if (wishlist == null) return null;
           
     
            var auction = wishlist.Auctions.FirstOrDefault(a => a.Id == auctionId);
            if (auction is null) return null;

            wishlist.Auctions.Remove(auction);
            await AddToWishlistAsync(wishlist);
            return wishlist;
        }
    }
}
