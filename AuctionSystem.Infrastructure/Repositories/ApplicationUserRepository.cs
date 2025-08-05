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
    }
}
