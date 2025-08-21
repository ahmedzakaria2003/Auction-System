using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.BidDTO
{
    public class BidDto
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }  
        public Guid BidderId { get; set; }  
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; } 
        public string BidderName { get; set; } = default!;  

    }
}
