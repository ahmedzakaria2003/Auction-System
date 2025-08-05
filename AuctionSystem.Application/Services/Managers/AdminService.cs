using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.AdminDTO;
using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Application.DTOS.BidDTO;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Application.Specification;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Shared;
using AuctionSystem.Shared.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Managers
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<AuctionHub> _hubContext;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper , IHubContext<AuctionHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<AuctionStatisticsDto> GetAuctionStatisticsAsync()
{
    var auctions = await _unitOfWork.Auctions.GetAllAuctionsAsync();

    if (auctions == null || !auctions.Any())
        throw new NotFoundException("No auctions found.");
   

            var statistics = new AuctionStatisticsDto
            {
                TotalAuctionsPaid = auctions.Count(a => a.IsPaid),
                TotalAuctionsUnPaid = auctions.Count(a => !a.IsPaid),
                TotalAuctions = auctions.Count(),
                TotalBids = auctions.Sum(a => a.Bids.Count),
                TotalRevenue = auctions.Sum(a => a.Bids.Sum(b => b.Amount)),
                TotalCanceled = auctions.Count(a => a.IsCanceled),
                OpenAuctions = auctions.Count(a => a.StartTime <= DateTime.UtcNow && a.EndTime >= DateTime.UtcNow && !a.IsCanceled),
                ClosedAuctions = auctions.Count(a => a.EndTime < DateTime.UtcNow && !a.IsCanceled),
                AuctionsByCategory = auctions
                    .GroupBy(a => a.Category.Name)
                    .Select(g => new CategoryAuctionStatsDto
                    {
                        Category = g.Key,
                        AuctionCount = g.Count(),
                        TotalRevenue = g.Sum(a => a.Bids.Sum(b => b.Amount))
                    }).ToList(),

                BiddersStats = auctions.SelectMany(a => a.Bids)
                    .GroupBy(b => b.BidderId)
                    .Select(g => new BidderStatsDto
                    {
                        BidderName = g.FirstOrDefault()?.Bidder?.FullName ?? "Unknown",
                        BidderId = g.Key,
                        TotalBids = g.Count(),
                        TotalAmountSpent = g.Sum(b => b.Amount)
                    }).OrderByDescending(b => b.TotalBids)
                    .FirstOrDefault(),

                MostBidAuction = auctions
                    .OrderByDescending(a => a.Bids.Count)
                    .Select(a => new MostBidAuctionDto
                    {
                        Title = a.Title,
                        Count = a.Bids.Count,
                        

                    })
                    .FirstOrDefault(),



            };

    return statistics;
}
        public async Task<IEnumerable<AuctionListDto>> GetSellersAuctions(AuctionQueryParams specParams, Guid userId , bool IsAdmin)
        {
            var spec = new AuctionSpecification(specParams, userId , IsAdmin == true);

            var auctions = await _unitOfWork.Auctions.ListAsync(spec);

            if (auctions is null || !auctions.Any())
            {
                throw new NotFoundException("No auctions found.");
            }

            var auctionDtos = _mapper.Map<IEnumerable<AuctionListDto>>(auctions);
            return auctionDtos;
        }



        public async Task<List<UserDto>> GetAllUsersAsync()
        {
           var users = await _unitOfWork.Users.GetAllUsersAsync();
            if (users is null )
            {
                throw new NotFoundException("No users found.");
            }
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;    
        }

        public async Task BanUserAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                throw new NotFoundException("User not found.");
            }
            user.IsBanned = true;
           await _unitOfWork.Users.UpdateAsync(user);
            await _hubContext.Clients.All.SendAsync("UserBanned", userId, user.FullName);

        }

        public async Task UnbanUserAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                throw new NotFoundException("User not found.");
            }
            user.IsBanned = false;
            await _unitOfWork.Users.UpdateAsync(user);
            await _hubContext.Clients.All.SendAsync("UserUnbanned", userId, user.FullName);

        }
    }
}
