using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Shared
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult()
        {
            
        }
        public PaginatedResult(int pageNumber, int pageSize, int totalCount, 
            IEnumerable<TEntity> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Count = totalCount;
            Data = data;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<TEntity> Data { get; set; }
    }

}
