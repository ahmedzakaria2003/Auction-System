using AuctionSystem.Application.DTOS.AdminDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface ISellerService
    {

        Task<AuctionStatisticsDto> GetSellerStatistics(Guid userId);

    }
}
