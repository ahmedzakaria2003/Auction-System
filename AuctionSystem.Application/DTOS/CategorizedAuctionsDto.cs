using AuctionSystem.Application.DTOS.AuctionProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS
{
    public class CategorizedAuctionsDto
    {
        public List<AuctionListDto> OpenAuctions { get; set; } = new();
        public List<AuctionListDto> ClosedAuctions { get; set; } = new();
        public List<AuctionListDto> CancelledAuctions { get; set; } = new();
    }

}
