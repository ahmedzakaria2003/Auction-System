using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AdminDTO
{
    public class BidderStatsDto
    {
        public string? BidderName { get; set; }
        public Guid BidderId { get; set; }
        public int TotalBids { get; set; }
        public decimal TotalAmountSpent { get; set; }
    }
}
