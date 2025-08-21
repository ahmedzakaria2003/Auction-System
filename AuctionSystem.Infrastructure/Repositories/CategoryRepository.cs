using AuctionSystem.Application.Contracts;
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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AuctionDbContext _context;

        public CategoryRepository(AuctionDbContext context):base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Category>> GetAllCategoriesWithAuctionsAsync()
        {
            return await _context.Categories.Include(c => c.Auctions)
                    .ThenInclude(a => a.Images)
                .Include(c => c.Auctions)
                    .ThenInclude(a => a.CreatedBy)
                     .Include(c => c.Auctions)
                    .ThenInclude(a => a.Bids)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(c => c.Auctions)
               
                    .ToListAsync();
        }

    }
}
