using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class AuctionSpecificationForCount : BaseSpecification<Auction>
    {
        public AuctionSpecificationForCount(AuctionQueryParams specParams, Guid currentUserId, bool isAdmin)
            : base(a =>
                (string.IsNullOrEmpty(specParams.Status) ||
                    (specParams.Status == "open" && !a.IsCanceled && a.EndTime > DateTime.UtcNow) ||
                    (specParams.Status == "closed" && !a.IsCanceled && a.EndTime <= DateTime.UtcNow) ||
                    (specParams.Status == "canceled" && a.IsCanceled)) &&
                (string.IsNullOrEmpty(specParams.Search) || a.Title.ToLower().Contains(specParams.Search)) &&
                (!specParams.MinPrice.HasValue || a.StartingPrice >= specParams.MinPrice.Value) &&
                (!specParams.MaxPrice.HasValue || a.StartingPrice <= specParams.MaxPrice.Value) &&
                (!specParams.IsEndingSoon.HasValue ||
                    (specParams.IsEndingSoon.Value &&
                        a.EndTime > DateTime.UtcNow && a.EndTime <= DateTime.UtcNow.AddHours(4))) &&
                (!isAdmin || a.CreatedBy.Id != currentUserId)
            )
        {
        }
    }

}
