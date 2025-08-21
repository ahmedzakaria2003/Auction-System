using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS
{
    public class AuctionQueryParamsDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Search { get; set; }
        public string Status { get; set; } = "";
        public Guid? CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public AuctionSorting Sort { get; set; } = AuctionSorting.NameAsc;
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public bool? IsEndingSoon { get; set; }

    }

}
