using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace AuctionSystem.Application.Services.Managers
{
    public class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<AuctionHub> _hubContext;

        public IAuctionService AuctionService { get; }
        public IBidService BidService { get; }
        public ICategoryService CategoryService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public IProfileService ProfileService { get; }
        public IPayementService PayementService { get; }

        public IDepositService DepositService {  get; }

        public IWishlistService WishlistService { get; }

        public IAdminService AdminService { get; }
        public ISellerService SellerService { get; }
        public ISellerFeedbackService SellerFeedbackService { get; }

        public ServiceManager(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IOtpService otpService,
            IFileService fileService,
            IConfiguration configuration,
            IWishlistRepository wishlist,
            IHubContext<AuctionHub> hubContext


            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            AuctionService = new AuctionService(_unitOfWork, _mapper, fileService , hubContext);
            BidService = new BidService(_unitOfWork, _mapper, userManager , hubContext);
            CategoryService = new CategoryService(_unitOfWork, _mapper);
            AuthenticationService = new AuthenticationService(userManager, _unitOfWork, otpService, tokenService);
            ProfileService = new ProfileService(_unitOfWork, _mapper);
            PayementService = new PaymentService(configuration , _unitOfWork);
            DepositService = new DepositService(configuration , _unitOfWork);
            WishlistService = new WishlistService(wishlist);
            AdminService = new AdminService(_unitOfWork, _mapper, hubContext);
            SellerService = new SellerService(_unitOfWork, _mapper);
            SellerFeedbackService = new SellerFeedbackService(_unitOfWork, _mapper);
        }
    }
}

