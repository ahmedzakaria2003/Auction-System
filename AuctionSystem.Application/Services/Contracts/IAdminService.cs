using AuctionSystem.Application.DTOS.AdminDTO;
using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Application.Specification;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IAdminService
    {
        Task<IEnumerable<AuctionListDto>> GetSellersAuctions(AuctionQueryParams specParams, Guid userId , bool IsAdmin);
        Task<AuctionStatisticsDto> GetAuctionStatisticsAsync();
        Task<List<UserDto>> GetAllUsersAsync();
        Task BanUserAsync(Guid userId);
        Task UnbanUserAsync(Guid userId);

    }
}
