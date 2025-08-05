using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class AuctionSpecification : BaseSpecification<Auction>
    {
        public AuctionSpecification(AuctionQueryParams specParams, Guid currentUserId, bool isAdmin)
            : base(a =>
                (string.IsNullOrEmpty(specParams.Status) ||
                    (specParams.Status == "open" && !a.IsCanceled && a.EndTime > DateTime.UtcNow) ||
                    (specParams.Status == "closed" && !a.IsCanceled && a.EndTime <= DateTime.UtcNow) ||
                    (specParams.Status == "canceled" && a.IsCanceled)) &&
                            (string.IsNullOrEmpty(specParams.Search) || a.Title.ToLower().Contains(specParams.Search)) &&

            (!specParams.MinPrice.HasValue ||
            a.StartingPrice >= specParams.MinPrice.Value) &&
            (!specParams.MaxPrice.HasValue ||
            a.StartingPrice <= specParams.MaxPrice.Value) &&
        (!specParams.IsEndingSoon.HasValue ||
 (specParams.IsEndingSoon.Value &&
  a.EndTime > DateTime.UtcNow && a.EndTime <= DateTime.UtcNow.AddHours(4)))&&

                (!isAdmin || a.CreatedBy.Id != currentUserId)
            )
        {
            AddInclude(a => a.CreatedBy);
            AddInclude(a => a.Images);
            AddInclude(a => a.Category);
            AddInclude(a => a.Bids);
            AddInclude(a => a.Winner);
            AddOrderByDescending(a => a.EndTime);

            switch (specParams.Sort)
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

            ApplyPaging((specParams.PageNumber - 1) * specParams.PageSize, specParams.PageSize);
        }
    }
}