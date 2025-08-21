using AuctionSystem.Application.Contracts;
using AuctionSystem.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuctionDbContext _context;

        public IAuctionRepository Auctions { get; }
        public IBidRepository Bids { get; }
        public ICategoryRepository Categories { get; }
        public IApplicationUserRepository Users { get;  } 

        public IRefreshTokenRepository refreshToken { get; }

        public IOtpRepository OtpRepository { get; }

        public IUserDepositRepository Deposits { get; }


        public ISellerFeedbackRepository SellerFeedbacks { get; }

        public UnitOfWork(
            AuctionDbContext context,
            IAuctionRepository auctionRepo,
            IBidRepository bidRepo,
            ICategoryRepository categoryRepo,
            IRefreshTokenRepository RefreshToken,
            IApplicationUserRepository users,
            IOtpRepository otp,
            IUserDepositRepository depositRepository,
            ISellerFeedbackRepository  feedbackRepository
            )
        {
            _context = context;
            Auctions = auctionRepo;
            Bids = bidRepo;
            Categories = categoryRepo;
            refreshToken = RefreshToken;
            Users = users; 
            OtpRepository = otp;
            Deposits = depositRepository;
            SellerFeedbacks = feedbackRepository;
        }


        public Task<int> SaveChangesAsync()
        {
            return  _context.SaveChangesAsync();
        }
    }
}
