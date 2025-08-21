using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class AuctionsByCategorySpecification : BaseSpecification<Auction>
    {
        public AuctionsByCategorySpecification(AuctionQueryParams queryParams, Guid categoryId)
          : base(a =>
              a.CategoryId == categoryId &&
              !a.IsCanceled &&
              a.EndTime > DateTime.UtcNow &&
              a.StartTime <= DateTime.UtcNow &&
              (string.IsNullOrEmpty(queryParams.Search) || a.Title.ToLower().Contains(queryParams.Search.ToLower())) &&
              (!queryParams.MinPrice.HasValue || a.StartingPrice >= queryParams.MinPrice.Value) &&
              (!queryParams.MaxPrice.HasValue || a.StartingPrice <= queryParams.MaxPrice.Value) &&
              (
                  !queryParams.IsEndingSoon.HasValue || 
                  (queryParams.IsEndingSoon.Value && a.EndTime <= DateTime.UtcNow.AddHours(4)) 
              )
          )
        {
            AddInclude(a => a.Images);
            AddInclude(a => a.Category);
            AddInclude(a => a.Bids);
            AddInclude(a => a.CreatedBy);

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
