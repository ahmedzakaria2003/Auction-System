using AuctionSystem.Application.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly AuctionDbContext _context;

        public ApplicationUserRepository(AuctionDbContext context)
        {
            _context = context;
        }

     
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<ApplicationUser?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

       

        public Task UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            return _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ApplicationUser>> ListAsync(ISpecification<ApplicationUser> spec)
        {
            var query = ApplySpecification(spec);
            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<ApplicationUser> spec)
        {
            var query = ApplySpecification(spec);
            return await query.CountAsync();
        }

        private IQueryable<ApplicationUser> ApplySpecification(ISpecification<ApplicationUser> spec)
        {
            IQueryable<ApplicationUser> query = _context.Users.AsQueryable();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            foreach (var include in spec.Includes)
            {
                query = query.Include(include);
            }

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }
    }
}
    

