using AuctionSystem.Application.Specification;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;

public class AuctionByCreatorSpecification : BaseSpecification<Auction>
{
    public AuctionByCreatorSpecification(AuctionQueryParams specParams, Guid userId)
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
             
            (!specParams.CategoryId.HasValue || a.CategoryId == specParams.CategoryId.Value)
                   &&
    (!specParams.StartDate.HasValue || a.StartTime.Date >= specParams.StartDate.Value.Date)&&
            a.CreatedById == userId
        )
    {
        AddInclude(a => a.Images);
        AddInclude(a => a.Category);
        AddInclude(a => a.Bids);
        AddInclude(a => a.CreatedBy);

        ApplyPaging((specParams.PageNumber - 1) * specParams.PageSize, specParams.PageSize);
    }
}
