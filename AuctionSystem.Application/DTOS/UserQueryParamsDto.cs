using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS
{
    public class UserQueryParamsDto
    {
        public string? Search { get; set; }
        public string? UserType { get; set; } // seller أو bidder
        public string? Status { get; set; }   // active أو banned
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
