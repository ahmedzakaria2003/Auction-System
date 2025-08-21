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
               (string.IsNullOrEmpty(specParams.Search) || a.Title.ToLower().Contains(specParams.Search.ToLower()))
            &&
            (!specParams.CategoryId.HasValue || a.CategoryId == specParams.CategoryId.Value)

              && (!isAdmin || a.CreatedBy.Id != currentUserId)
            )
        {
        }
    }

}
