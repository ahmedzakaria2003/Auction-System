using AuctionSystem.Application.DTOS.BidDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IBidService
    {
        Task<bool> AddBidAsync(AddBidDto bidDto , Guid userId);  
        Task<IEnumerable<BidDto>> GetBidsForAuctionAsync(Guid auctionId);     
        Task<BidDto> GetHighestBidForAuctionAsync(Guid auctionId);            
    }

}
