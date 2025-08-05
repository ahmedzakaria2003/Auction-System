using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;

namespace AuctionSystem.Application.Specification
{
    public class AuctionWithFiltersSpecification : BaseSpecification<Auction>
    {
        public AuctionWithFiltersSpecification(AuctionQueryParams queryParams)
            : base(a =>
              !a.IsCanceled &&
               a.EndTime > DateTime.UtcNow &&
               a.StartTime <= DateTime.UtcNow &&
                          (string.IsNullOrEmpty(queryParams.Status) ||
            (queryParams.Status == "open" && !a.IsCanceled && a.EndTime > DateTime.UtcNow) ||
            (queryParams.Status == "closed" && !a.IsCanceled && a.EndTime <= DateTime.UtcNow) ||
            (queryParams.Status == "canceled" && a.IsCanceled)
        ) &&
              (string.IsNullOrEmpty(queryParams.Search) 
            || a.Title.ToLower().Contains(queryParams.Search.ToLower())) &&
             (!queryParams.MinPrice.HasValue || 
            a.StartingPrice >= queryParams.MinPrice.Value) &&
            (!queryParams.MaxPrice.HasValue ||
            a.StartingPrice <= queryParams.MaxPrice.Value) &&
            (!queryParams.IsEndingSoon.HasValue ||
          (queryParams.IsEndingSoon.Value &&
        a.EndTime > DateTime.UtcNow && a.EndTime <= DateTime.UtcNow.AddHours(4)))

            )
        {
            AddInclude(a => a.CreatedBy);
            AddInclude(a => a.Images);
            AddInclude(a => a.Category);
            AddInclude(a => a.Bids);
            AddInclude("Bids.Bidder");  
            AddInclude(a => a.Winner);


            switch (queryParams.Sort)
            {
                case AuctionSorting.NameAsc:
                    AddOrderBy(a => a.Title);
                    break;
                case AuctionSorting.NameDesc:
                    AddOrderByDescending(a => a.Title);
                    break;
                case AuctionSorting.PriceAsc:
                    AddOrderBy(a => a.StartingPrice);
                    break;
                case AuctionSorting.PriceDesc:
                    AddOrderByDescending(a => a.StartingPrice);
                    break;
            
            }

         ApplyPaging((queryParams.PageNumber - 1) * queryParams.PageSize, queryParams.PageSize);
        }
    }
}
