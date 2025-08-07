using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.CategoryProfile
{
    public class CategoryWithAuctionsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public PaginatedResult<AuctionListDto> PagedAuctions { get; set; } = default!;
    }
}
