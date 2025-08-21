using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class AuctionImage : BaseEntity
    {
        public string FileName { get; set; } = default!;
        public Guid AuctionId { get; set; }
        public Auction? Auction { get; set; }
    }

}
