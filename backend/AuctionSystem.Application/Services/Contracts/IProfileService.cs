using AuctionSystem.Application.DTOS.ProfileDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
 public interface IProfileService
    {
        Task<IEnumerable<AuctionWonDto>> GetWonAuctionAsync(Guid userId);
        Task<IEnumerable<ActiveBiddingDto>> GetActiveBiddingAsync(Guid userId); 
    }
}
