using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(AuctionQueryParams queryParams):
            base(c => 
                (string.IsNullOrEmpty(queryParams.Search) || c.Name.ToLower().Contains
            (queryParams.Search.ToLower())) 
            )
        {

            ApplyPaging((queryParams.PageNumber - 1) * queryParams.PageSize, queryParams.PageSize);

        }
    }
}