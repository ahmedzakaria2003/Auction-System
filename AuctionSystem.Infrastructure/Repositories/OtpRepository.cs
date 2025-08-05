using AuctionSystem.Application.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Data;
using AuctionSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class OtpRepository : GenericRepository<Otp>,IOtpRepository
{
    private readonly AuctionDbContext _context;

    public OtpRepository(AuctionDbContext context) : base(context)
    {
        _context = context;
    }





    public async Task<Otp?> GetLatestValidOtpAsync(string email)
    {
        return await _context.Otps
            .Where(x => x.UserEmail == email && x.ExpirationDate > DateTime.UtcNow)
            .OrderByDescending(x => x.ExpirationDate)
            .FirstOrDefaultAsync();
    }

   

   

  
}
