    using AuctionSystem.Application.Contracts;
    using AuctionSystem.Application.DTOS.SellerFeedbackDTO;
    using AuctionSystem.Application.Services.Contracts;
    using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace AuctionSystem.Application.Services.Managers
    {
        public class SellerFeedbackService : ISellerFeedbackService
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public SellerFeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task AddFeedbackAsync(SellerFeedbackDto feedbackDto)
            {
                var auction = await _unitOfWork.Auctions.GetAuctionWithDetailsAsync(feedbackDto.AuctionId);
                if (auction == null)
                    throw new BadRequestException("Auction not found.");

                if (auction.WinnerId != feedbackDto.BidderId || !auction.IsPaid)
                    throw new UnauthorizedException("Only the winning bidder who paid can leave feedback.");

                feedbackDto.SellerId = auction.CreatedById;

                var existingFeedbacks = await _unitOfWork.SellerFeedbacks
                .GetFeedbacksForSellerAsync(feedbackDto.SellerId);
                if (existingFeedbacks.Any(f => f.BidderId == feedbackDto.BidderId 
                && f.AuctionId == feedbackDto.AuctionId))
                    throw new BadRequestException("Feedback " +
                     "already submitted for this auction by this bidder.");

                var feedback = _mapper.Map<SellerFeedback>(feedbackDto);
                await _unitOfWork.SellerFeedbacks.AddFeedbackAsync(feedback);
            }

            public async Task<List<SellerFeedbackDto>> GetFeedbacksForSellerAsync(Guid sellerId)
            {
                var feedbacks = await _unitOfWork.SellerFeedbacks.GetFeedbacksForSellerAsync(sellerId);
                if (feedbacks == null || !feedbacks.Any())
                    throw new NotFoundException("No feedback found for this seller.");
            return _mapper.Map<List<SellerFeedbackDto>>(feedbacks);
            }

            public async Task<double> GetAverageRatingForSellerAsync(Guid sellerId)
            {
                return await _unitOfWork.SellerFeedbacks.GetAverageRatingForSellerAsync(sellerId);
            }
        public async Task<bool> HasUserRatedAuctionAsync(Guid userId, Guid auctionId)
        {
            return await _unitOfWork.SellerFeedbacks.HasUserRatedAuctionAsync(auctionId, userId);
        }



    }
}