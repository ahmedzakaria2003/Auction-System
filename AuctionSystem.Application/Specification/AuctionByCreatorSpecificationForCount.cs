using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class AuctionByCreatorSpecificationForCount : BaseSpecification<Auction>
    {
        public AuctionByCreatorSpecificationForCount(AuctionQueryParams specParams, Guid userId)
    : base(a =>
        (
            string.IsNullOrEmpty(specParams.Status) ||
            (specParams.Status == "open" && !a.IsCanceled && a.EndTime > DateTime.UtcNow) ||
            (specParams.Status == "closed" && !a.IsCanceled && a.EndTime <= DateTime.UtcNow) ||
            (specParams.Status == "canceled" && a.IsCanceled)
        )
        &&
        (string.IsNullOrEmpty(specParams.Search) || a.Title.ToLower().Contains(specParams.Search.ToLower()))
        &&
        (!specParams.CategoryId.HasValue || a.CategoryId == specParams.CategoryId.Value)
        &&
        a.CreatedById == userId
    )
        {
        }
    }

}
