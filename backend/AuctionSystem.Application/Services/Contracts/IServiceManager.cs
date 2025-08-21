using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IServiceManager
    {
        public IAuctionService AuctionService { get; }
        public IBidService BidService { get; }
        public ICategoryService CategoryService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public IProfileService ProfileService { get; }
        public IPayementService PayementService { get; }
        public IDepositService DepositService { get; }
        public IWishlistService WishlistService { get; }
        public IAdminService AdminService { get; }
        public ISellerService SellerService { get; }
        public ISellerFeedbackService SellerFeedbackService { get; }
    }
}
