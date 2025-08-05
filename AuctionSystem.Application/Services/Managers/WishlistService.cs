using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Infrastructure.RedisModels;
using AuctionSystem.Domain.Exceptions;
using System.Linq;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;

    public WishlistService(IWishlistRepository wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }

    public async Task<ICollection<AuctionListDto>> GetWishlistAsync(string key)
    {
        var wishlist = await _wishlistRepository.GetBidderWishlistAsync(key);

        if (wishlist == null)
            throw new NotFoundException("Wishlist not found for the given bidder.");

        return wishlist.Auctions ?? new List<AuctionListDto>(); // Return an empty list if no auctions
    }

    public async Task<ICollection<AuctionListDto>> AddToWishlistAsync(AuctionListDto dto, string key)
    {
        var wishlist = await _wishlistRepository.GetBidderWishlistAsync(key)
                       ?? new BidderWishlist { Id = key };

        // Prevent adding duplicates to the wishlist
        if (!wishlist.Auctions.Any(x => x.Id == dto.Id))
        {
            wishlist.Auctions.Add(dto);
        }
        else
        {
            throw new BadRequestException("This auction is already in the wishlist.");
        }

        var updated = await _wishlistRepository.AddToWishlistAsync(wishlist);

        if (updated == null)
            throw new BadRequestException("Failed to update the wishlist.");

        return updated.Auctions ?? new List<AuctionListDto>();
    }

    public async Task<ICollection<AuctionListDto>> RemoveFromWishlistAsync(string key, Guid auctionId)
    {
        var wishlist = await _wishlistRepository.GetBidderWishlistAsync(key);

        if (wishlist == null)
            throw new NotFoundException("Wishlist not found for the given bidder.");

        var auctionToRemove = wishlist.Auctions.FirstOrDefault(a => a.Id == auctionId);
        if (auctionToRemove == null)
            throw new NotFoundException("Auction not found in the wishlist.");

        var updated = await _wishlistRepository.RemoveFromWishlistAsync(key, auctionId);

        if (updated == null)
            throw new BadRequestException("Failed to update the wishlist after removal.");

        return updated.Auctions ?? new List<AuctionListDto>();
    }
}
