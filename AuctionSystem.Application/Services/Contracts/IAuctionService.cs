using AuctionSystem.Application.DTOS.AuctionDTO;
using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IAuctionService
    {
        Task<IEnumerable<AuctionListDto>> GetAllAuctionsAsync();
        Task<IEnumerable<AuctionListDto>> GetActiveAuctionsAsync(AuctionQueryParams queryParams);
        Task<AuctionDetailsDto?> GetAuctionDetailsAsync(Guid auctionId);
        Task<IEnumerable<AuctionListDto>> GetAuctionsByCreatorAsync(AuctionQueryParams queryParams , Guid userId);
        Task<Guid> CreateAuctionAsync(CreateAuctionDto dto , Guid userId);
        Task<bool> UpdateAuctionAsync(Guid auctionId, UpdateAuctionDto dto , Guid userId);
        Task<bool> DeleteAuctionAsync(Guid auctionId , Guid userId);
        Task<bool>CancelAuctionAsync(Guid auctionId);
        Task<WinnerDto> DeclareWinnerAsync(Guid auctionId);
        Task<IEnumerable<Auction>> GetAuctionsThatEndedWithoutWinnerAsync();
        Task<IEnumerable<AuctionListDto>> GetHotAuctionsAsync(int take = 10);
        Task<IEnumerable<AuctionListDto>> GetRecommendedAuctionsForBidderAsync(Guid userId);


    }

}
