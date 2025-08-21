using AuctionSystem.Application.DTOS.PaymentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IDepositService
    {
        Task<PaymentIntentDTO> CreateDepositIntentAsync(Guid userId , Guid auctionId);
        Task HandleStripeWebhookAsync(string requestBody, string signature);
        Task<bool> HasUserPaidDepositAsync(Guid userId, Guid auctionId);

        Task<bool> ConfirmDepositIntentAsync(string paymentIntentId); 

    }

}
