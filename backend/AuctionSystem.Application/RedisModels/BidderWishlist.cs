using AuctionSystem.Application.DTOS.AuctionProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.RedisModels
{
    public class BidderWishlist
    {
        public string Id { get; set; } = null!;

        public ICollection<AuctionListDto> Auctions { get; set; } = new List<AuctionListDto>();
    }
}
