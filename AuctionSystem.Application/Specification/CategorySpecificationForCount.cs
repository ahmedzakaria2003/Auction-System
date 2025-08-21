using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class CategorySpecificationForCount : BaseSpecification<Category>
    {
        public CategorySpecificationForCount(AuctionQueryParams queryParams) :
            base(c =>
                (string.IsNullOrEmpty(queryParams.Search) || c.Name.ToLower().Contains
            (queryParams.Search.ToLower()))
            )
        {

        }

            }  

}
