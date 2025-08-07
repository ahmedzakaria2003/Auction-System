using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class AuctionWithFiltersSpecificationForCount : BaseSpecification<Auction>
    {
        public AuctionWithFiltersSpecificationForCount(AuctionQueryParams queryParams)
            : base(a =>
                !a.IsCanceled &&
                a.EndTime > DateTime.UtcNow &&
                a.StartTime <= DateTime.UtcNow &&
                (string.IsNullOrEmpty(queryParams.Status) ||
                    (queryParams.Status == "open" && !a.IsCanceled && a.EndTime > DateTime.UtcNow) ||
                    (queryParams.Status == "closed" && !a.IsCanceled && a.EndTime <= DateTime.UtcNow) ||
                    (queryParams.Status == "canceled" && a.IsCanceled)) &&
                (string.IsNullOrEmpty(queryParams.Search) || a.Title.ToLower().Contains(queryParams.Search.ToLower())) &&
                (!queryParams.MinPrice.HasValue || a.StartingPrice >= queryParams.MinPrice.Value) &&
                (!queryParams.MaxPrice.HasValue || a.StartingPrice <= queryParams.MaxPrice.Value) &&
                (!queryParams.IsEndingSoon.HasValue ||
                    (queryParams.IsEndingSoon.Value &&
                        a.EndTime > DateTime.UtcNow && a.EndTime <= DateTime.UtcNow.AddHours(4)))
            )
        {
        }
    }

}
