using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Shared
{
    public class UserQueryParams
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 15;

        public string? Search { get; set; } 
        public string? UserType { get; set; } 
        public string? Status { get; set; } 
        public int PageNumber { get; set; } = 1;

        private int pageSize = DefaultPageSize;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }

}
