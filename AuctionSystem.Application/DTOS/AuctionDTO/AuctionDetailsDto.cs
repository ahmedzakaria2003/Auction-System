using AuctionSystem.Application.DTOS.BidDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AuctionProfile
{
    public class AuctionDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal StartingPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CategoryName { get; set; } = default!;
        public string SellerName { get; set; } = default!;
        public Guid SellerId { get; set; }
        public int TotalBids { get; set; }
        public string? WinnerName { get; set; }
        public List<BidDto> Bids { get; set; } = new();
        public decimal HighestBidAmount { get; set; }
        public string? HighestBidderName { get; set; }
        public List<string> ItemImageUrls { get; set; } = new();

    }
}
