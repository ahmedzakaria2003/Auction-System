using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.ProfileDTO
{
    public class ActiveBiddingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public decimal? CurrentHighestBid { get; set; }
        public decimal? YourBid { get; set; }
        public DateTime EndTime { get; set; }
        public List<string> Images { get; set; } = [];
    }

}
