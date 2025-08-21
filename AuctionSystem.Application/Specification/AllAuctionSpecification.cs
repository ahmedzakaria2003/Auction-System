using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class AllAuctionSpecifications : BaseSpecification<Auction>
    {
        public  AllAuctionSpecifications(AuctionQueryParams queryParams)
                : base(a =>

                          (string.IsNullOrEmpty(queryParams.Status) ||
            (queryParams.Status == "open" && !a.IsCanceled && a.EndTime > DateTime.UtcNow) ||
            (queryParams.Status == "closed" && !a.IsCanceled && a.EndTime <= DateTime.UtcNow) ||
            (queryParams.Status == "canceled" && a.IsCanceled)
        ) &&
              (string.IsNullOrEmpty(queryParams.Search) || a.Title.ToLower().
                Contains(queryParams.Search.ToLower()))
            &&
            (!queryParams.CategoryId.HasValue || a.CategoryId == queryParams.CategoryId.Value)
                   &&
    (!queryParams.StartDate.HasValue || a.StartTime.Date >= queryParams.StartDate.Value.Date)
&&
        (!queryParams.EndDate.HasValue || a.EndTime.Date <= queryParams.EndDate.Value.Date.AddDays(1).AddTicks(-1))

            )
        {
        AddInclude(a => a.CreatedBy);
        AddInclude(a => a.Category);
        AddInclude(a => a.Bids);




         ApplyPaging((queryParams.PageNumber - 1) * queryParams.PageSize, queryParams.PageSize);
    } 
}
  
}
