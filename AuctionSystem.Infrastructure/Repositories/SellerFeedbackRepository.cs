using AuctionSystem.Application.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class SellerFeedbackRepository : ISellerFeedbackRepository
{
    private readonly AuctionDbContext _context;

    public SellerFeedbackRepository(AuctionDbContext context)
    {
        _context = context;
    }

    public async Task AddFeedbackAsync(SellerFeedback feedback)
    {
        _context.SellerFeedbacks.Add(feedback);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SellerFeedback>> GetFeedbacksForSellerAsync(Guid sellerId)
    {
        return await _context.SellerFeedbacks
            .Where(f => f.SellerId == sellerId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingForSellerAsync(Guid sellerId)
    {
        return await _context.SellerFeedbacks
            .Where(f => f.SellerId == sellerId)
            .AverageAsync(f => (double?)f.Rating) ?? 0.0;
    }

    public async Task<bool> HasUserRatedAuctionAsync(Guid auctionId, Guid bidderId)
    {
        return await _context.SellerFeedbacks
                   .AnyAsync(f => f.AuctionId == auctionId && f.BidderId == bidderId);
    }

}