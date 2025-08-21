using AuctionSystem.Application.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Repositories
{
  public class BidRepoistory : GenericRepository<Bid>, IBidRepository
    {
        private readonly AuctionDbContext _context;

        public BidRepoistory(AuctionDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bid>> GetBidsForAuctionAsync(Guid auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId).Include(a=>a.Bidder).ToListAsync();
        }

        public Task<Bid?> GetHighestBidForAuctionAsync(Guid auctionId)
        {
           
            return _context.Bids
                .Where(b => b.AuctionId == auctionId).Include(a => a.Bidder).Include(b=>b.Auction)
                .OrderByDescending(b=> b.Amount)
                .FirstOrDefaultAsync();

        }

        public async Task<bool> HasUserBidAsync(Guid auctionId, Guid userId)
        {
            return await _context.Bids
                .AnyAsync(b=>b.AuctionId == auctionId && b.BidderId == userId);
        }
    }
}
