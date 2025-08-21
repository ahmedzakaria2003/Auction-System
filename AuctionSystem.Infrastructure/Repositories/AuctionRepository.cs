using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.ProfileDTO;
using AuctionSystem.Application.Specification;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Data;
using AuctionSystem.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class AuctionRepository : GenericRepository<Auction>, IAuctionRepository
    {
        private readonly AuctionDbContext _context;
        public AuctionRepository(AuctionDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync()
        {

            return await _context.Auctions
               .Include(a => a.CreatedBy)
                 .Include(a => a.Images)
                 .Include(a => a.Category)
                 .Include(a => a.Bids)
                 .ThenInclude(a => a.Bidder)
                 .ToListAsync();
        }




        public async Task<IEnumerable<Auction>> GetAuctionsByCreatorIdAsync(Guid userId)
        {
            return await _context.Auctions
                .Where(a => a.CreatedById == userId).Include(a => a.CreatedBy)
                 .Include(a => a.Images)
                 .Include(a => a.Category)
                 .Include(a => a.Bids)
                                  .ThenInclude(a => a.Bidder)

                .ToListAsync();
        }

        public async Task<Auction?> GetAuctionWithDetailsAsync(Guid auctionId)
        {
            return await _context.Auctions
                 .Include(a => a.CreatedBy)
                 .Include(a => a.Images)
                 .Include(a => a.Winner)
                 .Include(a => a.Category)
                 .Include(a => a.Bids)
                 .ThenInclude(a=>a.Bidder)
                 .FirstOrDefaultAsync(a => a.Id == auctionId);
        }
        public async Task<IEnumerable<Auction>> GetAuctionsByCategoryAsync(Guid categoryId)
        {
            return await _context.Auctions
                .Where(a => a.CategoryId == categoryId)
                .ToListAsync();
        }

        public Task<Auction?> GetAuctionWithImagesAsync(Guid auctionId)
        {
            return _context.Auctions
                   .Include(a => a.Images).Include(a => a.Bids)

                   .FirstOrDefaultAsync(a => a.Id == auctionId);
        }

        public async Task<IEnumerable<Auction>> GetWonAuctionAsync(Guid userId)
        {
            return await _context.Auctions.Include(a => a.Images).Include(a=>a.Winner)

                .Include(a=> a.Category).Where(a=>a.WinnerId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetActiveBiddingAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            return await _context.Auctions.Include(a => a.Images)
               .Include(a => a.Category).Include(a => a.Bids)
               .Where(a => a.StartTime <= now &&  a.EndTime >= now && a.Bids.Any(b=>b.BidderId == userId))
               .ToListAsync();
        }

        public async Task<Auction?> GetByPaymentIntentIdAsync(string paymentIntentId)
        {
            return await _context.Auctions.FirstOrDefaultAsync(a=>a.PaymentIntentId == paymentIntentId);
        }

        public async Task<IEnumerable<Auction>> GetAuctionsThatEndedWithoutWinnerAsync()
        {
            return await _context.Auctions
                .Where(a => a.EndTime <= DateTime.UtcNow &&
                            a.WinnerId == null &&
                            !a.IsCanceled)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetRecommendedAuctionsForBidderAsync(Guid bidderId)
        {
            var categoryIds = await _context.Bids.Where(b => b.BidderId == bidderId)
                 .Select(b => b.Auction.CategoryId).Distinct().ToListAsync();

            return await _context.Auctions
                .Where(a => categoryIds.Contains(a.CategoryId)
                     && a.StartTime <= DateTime.UtcNow &&
                      a.EndTime >= DateTime.UtcNow &&
                       !a.IsCanceled)
                .Include(a => a.Images)
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.CreatedBy)
                   .ToListAsync();
        }
        public async Task<IEnumerable<Auction>> GetHotAuctionsAsync(int take = 10)
        {
            return await _context.Auctions
                .Where(a => !a.IsCanceled && a.StartTime <= DateTime.UtcNow &&
                      a.EndTime >= DateTime.UtcNow && a.Bids.Any() )
                .OrderByDescending(a => a.Bids.Count)
                .Take(take)
                .Include(a => a.CreatedBy)
                .Include(a => a.Images)
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
                .Include(a => a.Winner)
                .ToListAsync();
        }



    }
}
