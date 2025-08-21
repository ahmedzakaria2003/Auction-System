using AuctionSystem.Application.DTOS;
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
        Task<PaginatedResult<AuctionListDto>> GetSellersAuctions
            (AuctionQueryParamsDto queryParamsDto, Guid userId , bool IsAdmin);
        Task<AuctionStatisticsDto> GetAuctionStatisticsAsync();
        Task<PaginatedResult<UserDto>> GetAllUsersAsync(UserQueryParamsDto paramsDto);
        Task BanUserAsync(Guid userId);
        Task UnbanUserAsync(Guid userId);

    }
}
