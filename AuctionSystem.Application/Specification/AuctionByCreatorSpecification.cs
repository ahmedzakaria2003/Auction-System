using AuctionSystem.Application.Specification;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;

public class AuctionByCreatorSpecification : BaseSpecification<Auction>
{
    public AuctionByCreatorSpecification(AuctionQueryParams specParams, Guid userId)
        : base(a =>
    (
        (string.IsNullOrEmpty(specParams.Status) ||
            (specParams.Status == "open" && !a.IsCanceled && a.EndTime > DateTime.UtcNow) ||
            (specParams.Status == "closed" && !a.IsCanceled && a.EndTime <= DateTime.UtcNow) ||
            (specParams.Status == "canceled" && a.IsCanceled)
        )
    )&&
    (string.IsNullOrEmpty(specParams.Search) || a.Title.ToLower().Contains(specParams.Search)) &&
    a.CreatedById == userId &&
    (!specParams.MinPrice.HasValue || a.StartingPrice >= specParams.MinPrice.Value) &&
    (!specParams.MaxPrice.HasValue || a.StartingPrice <= specParams.MaxPrice.Value) &&
    (!specParams.IsEndingSoon.HasValue ||
        (specParams.IsEndingSoon.Value && !a.IsCanceled && a.EndTime > 
        DateTime.UtcNow && a.EndTime <= DateTime.UtcNow.AddHours(4)))  
    )
    {
        AddInclude(a => a.Images);
        AddInclude(a => a.Category);
        AddInclude(a => a.Bids);
        AddInclude(a => a.CreatedBy);

        switch (specParams.Sort)
        {
            case AuctionSorting.NameAsc:
                AddOrderBy(a => a.Title.ToLower());
                break;
            case AuctionSorting.NameDesc:
                AddOrderByDescending(a => a.Title.ToLower());
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
