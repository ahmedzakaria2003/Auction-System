using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.BidDTO;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Shared.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Managers
{
    public class BidService : IBidService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<AuctionHub> _hubContext;

        public BidService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, IHubContext<AuctionHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task<bool> AddBidAsync(AddBidDto bidDto, Guid userId)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(bidDto.AuctionId);
            if (auction == null)
                throw new NotFoundException($"Auction with ID {bidDto.AuctionId} not found.");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException("User not found.");

            if (user.IsBanned)
                throw new UnauthorizedException("You are banned from placing bids in this auction.");

            var userRole = await _userManager.GetRolesAsync(user);
            if (!userRole.Any(r => r == "Bidder"))
                throw new UnauthorizedException($"User with ID {userId} is not a bidder or does not have the correct role.");

            if (!await _unitOfWork.Deposits.HasPaidDepositeAsync(userId, bidDto.AuctionId))
                throw new BadRequestException("You must pay a deposit to participate in this auction.");

            if (DateTime.UtcNow < auction.StartTime || DateTime.UtcNow > auction.EndTime)
                throw new BadRequestException($"Auction {bidDto.AuctionId} is not active or does not exist.");

            if (auction.IsCanceled)
                throw new BadRequestException($"Auction with ID {bidDto.AuctionId} is canceled and cannot accept new bids.");

            var highestBid = await _unitOfWork.Bids.GetHighestBidForAuctionAsync(bidDto.AuctionId);
            if (bidDto.Amount <= (highestBid?.Amount ?? auction.StartingPrice))
                throw new BadRequestException($"Your bid of {bidDto.Amount} is too low. The current highest bid is {highestBid?.Amount ?? auction.StartingPrice}. Please place a higher bid.");

            var bid = _mapper.Map<Bid>(bidDto);
            bid.BidderId = userId;
            bid.BidTime = DateTime.UtcNow;

            await _unitOfWork.Bids.AddAsync(bid);

            auction.FinalPrice = bidDto.Amount;

            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.Group(auction.Id.ToString()).SendAsync("ReceiveBidNotification", bidDto.Amount, user.UserName);

            return true;
        }

        public async Task<IEnumerable<BidDto>> GetBidsForAuctionAsync(Guid auctionId)
        {
            var bids = await _unitOfWork.Bids.GetBidsForAuctionAsync(auctionId);
            if (bids == null || !bids.Any())
                throw new NotFoundException($"No bids found for auction with ID {auctionId}");

            return _mapper.Map<IEnumerable<BidDto>>(bids);
        }

        public async Task<BidDto> GetHighestBidForAuctionAsync(Guid auctionId)
        {
            var bid = await _unitOfWork.Bids.GetHighestBidForAuctionAsync(auctionId);
            if (bid == null)
                throw new NotFoundException($"No bids found for auction {auctionId}");

            return _mapper.Map<BidDto>(bid);
        }
    }
}

