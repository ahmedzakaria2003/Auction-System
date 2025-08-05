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
    public class UserDepositRepository : GenericRepository<UserDeposite>, IUserDepositRepository
    {
        private readonly AuctionDbContext _context;
        public UserDepositRepository(AuctionDbContext context) : base(context)
        {
            _context = context;
        }

       
        public async Task<UserDeposite?> GetByPaymentIntentIdAsync(string intentId)
        {
          return  await _context.UserDeposites
                .FirstOrDefaultAsync(d => d.PaymentIntentId == intentId);
           
        }

        public async Task<bool> HasPaidDepositeAsync(Guid userId, Guid auctionId)
        {
            return await _context.UserDeposites.AnyAsync(d => d.UserId == userId && d.AuctionId==auctionId&&d.IsPaid);
        }
    }
}
