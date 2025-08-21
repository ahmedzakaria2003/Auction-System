using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AdminDTO
{
    public class CategoryAuctionStatsDto
    {
        public string? Category { get; set; }
        public int AuctionCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
