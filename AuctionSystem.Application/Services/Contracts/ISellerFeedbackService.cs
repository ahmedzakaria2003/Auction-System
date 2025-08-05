using AuctionSystem.Application.DTOS.SellerFeedbackDTO;
using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface ISellerFeedbackService
    {
        Task AddFeedbackAsync(SellerFeedbackDto feedbackDto);
        Task<List<SellerFeedbackDto>> GetFeedbacksForSellerAsync(Guid sellerId);
        Task<double> GetAverageRatingForSellerAsync(Guid sellerId);
    }
}