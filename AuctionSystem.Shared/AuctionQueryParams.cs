using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Shared
{
        public class AuctionQueryParams
        {
            private const int DefaultPageSize = 5;
            private const int MaxPageSize = 10;

            public AuctionSorting Sort { get; set; }
          
            public string? Search { get; set; }
           public string Status { get; set; } = "";
           public int PageNumber { get; set; } = 1;

            private int pageSize = DefaultPageSize;
            public int PageSize
            {
                get => pageSize;
                set => pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
            public decimal? MaxPrice { get; set; }
            public decimal? MinPrice { get; set; } 
        public bool? IsEndingSoon { get; set; }  
        }
    }


