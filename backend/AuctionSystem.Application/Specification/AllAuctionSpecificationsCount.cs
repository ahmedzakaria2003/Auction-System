using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;

namespace AuctionSystem.Application.Specification
{
    public class AllAuctionSpecificationsCount : BaseSpecification<Auction>
    {
        public AllAuctionSpecificationsCount(AuctionQueryParams queryParams)
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
    (!queryParams.EndDate.HasValue || a.EndTime.Date <= queryParams.EndDate.Value.Date)

            )
            
        {
        }
    }
}
