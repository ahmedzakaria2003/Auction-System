using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.ProfileDTO
{
    public class AuctionWonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public decimal WinningBidAmount { get; set; } 
        public DateTime EndTime { get; set; }
        public bool IsPaid { get; set; }
        public List<string> Images { get; set; } = [];
    }

}
