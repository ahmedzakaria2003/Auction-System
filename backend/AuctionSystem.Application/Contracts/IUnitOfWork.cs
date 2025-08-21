using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Contracts
{
    public interface IUnitOfWork
    {
        IAuctionRepository Auctions { get; }
        IBidRepository Bids { get; }
        ICategoryRepository Categories { get; }


        IRefreshTokenRepository refreshToken { get; }

        IApplicationUserRepository Users { get; }

        IOtpRepository OtpRepository { get; }

        IUserDepositRepository Deposits { get; }


        ISellerFeedbackRepository SellerFeedbacks { get; }

        Task<int> SaveChangesAsync();
 

    }
}
