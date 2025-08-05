using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AuctionProfile
{
    public class AuctionListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public decimal StartingPrice { get; set; }
        public DateTime EndTime { get; set; }
        public string CategoryName { get; set; } = default!;
        public bool IsCanceled { get; set; } = false;
        public string SellerName { get; set; } = default!;

        public int BidsCount { get; set; }

        public List<string> ThumbnailImage { get; set; } = [];
        public string AuctionStatus { get; set; } = default!; // This can be "open", "closed", or "canceled"

    }
}
