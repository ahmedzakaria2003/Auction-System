using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.ProfileDTO;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Managers
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper)
          
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuctionWonDto>> GetWonAuctionAsync(Guid userId)
        {
            var winnerAuctions = await _unitOfWork.Auctions.GetWonAuctionAsync(userId);

            if (winnerAuctions == null || !winnerAuctions.Any())
                throw new NotFoundException("No won auctions found for this user.");
            return  _mapper.Map<IEnumerable<AuctionWonDto>>(winnerAuctions);
        }

        public async Task<IEnumerable<ActiveBiddingDto>> GetActiveBiddingAsync(Guid userId)
        {
            var auctions = await _unitOfWork.Auctions.GetActiveBiddingAsync(userId);
            if (auctions == null || !auctions.Any())
                throw new NotFoundException("No active bids found for this user.");
            return _mapper.Map<IEnumerable<ActiveBiddingDto>>
               (auctions, opt => opt.Items["UserId"] = userId);
           
        }

       
    }
}
