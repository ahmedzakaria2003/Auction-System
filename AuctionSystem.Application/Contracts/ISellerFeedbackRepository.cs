using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Contracts
{
    public interface ISellerFeedbackRepository
    {
        Task AddFeedbackAsync(SellerFeedback feedback);
        Task<List<SellerFeedback>> GetFeedbacksForSellerAsync(Guid sellerId);
        Task<double> GetAverageRatingForSellerAsync(Guid sellerId);
    }
}