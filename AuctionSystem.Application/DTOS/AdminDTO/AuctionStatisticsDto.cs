using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AdminDTO
{
    public class AuctionStatisticsDto
    {
        public int TotalAuctionsUnPaid { get; set; }
        public int TotalAuctionsPaid { get; set; }
        public int TotalAuctions { get; set; }
        public int TotalBids { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCanceled { get; set; }
        public int OpenAuctions { get; set; }  
        public int ClosedAuctions { get; set; }
        public List<CategoryAuctionStatsDto> AuctionsByCategory { get; set; } = [];
        public BidderStatsDto BiddersStats { get; set; } = default!;
        public MostBidAuctionDto MostBidAuction { get; set; } = default!;

    }

}
