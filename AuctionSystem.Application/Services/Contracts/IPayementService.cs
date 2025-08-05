using AuctionSystem.Application.DTOS.PaymentDTO;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IPayementService
    {
        Task<PaymentIntentDTO> CreatePaymentIntentForAuctionAsync(Guid auctionId);
        Task HandleStripeWebhookAsync(string json, string stripeSignature);

        Task<bool> ConfirmPaymentIntentAsync(string paymentIntentId);

    }
}
