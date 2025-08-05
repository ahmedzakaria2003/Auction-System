using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Infrastructure.RedisModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IWishlistService
    {
        Task<ICollection<AuctionListDto>> GetWishlistAsync(string key);
        Task<ICollection<AuctionListDto>> AddToWishlistAsync(AuctionListDto dto, string key);
        Task<ICollection<AuctionListDto>> RemoveFromWishlistAsync(string key, Guid auctionId);
    }

}
