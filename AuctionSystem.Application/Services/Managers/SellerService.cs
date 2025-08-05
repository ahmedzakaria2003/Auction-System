using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.AdminDTO;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Shared;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Managers
{
    public class SellerService : ISellerService
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SellerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<AuctionStatisticsDto> GetSellerStatistics( Guid userId)
        {
            var auctions = await _unitOfWork.Auctions.GetAuctionsByCreatorIdAsync( userId);
            if (auctions == null || !auctions.Any())
                throw new NotFoundException("No auctions Statics  found.");
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
    }
}
