using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.PaymentDTO;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Stripe;

public class DepositService : IDepositService
{
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _unitOfWork;

    public DepositService(IConfiguration config, IUnitOfWork unitOfWork)
    {
        _config = config;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentIntentDTO> CreateDepositIntentAsync(Guid userId , Guid auctionId)
    {
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

        if (await _unitOfWork.Deposits.HasPaidDepositeAsync(userId, auctionId))
            throw new BadRequestException("User has already paid the deposit."); 

        var service = new PaymentIntentService();
        var options = new PaymentIntentCreateOptions
        {
            Amount = 5000, 
            Currency = "egp",
            PaymentMethodTypes = ["card"]
        };

        var intent = await service.CreateAsync(options);

        var deposit = new UserDeposite
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AuctionId =auctionId,
            PaymentIntentId = intent.Id,
            IsPaid = false
        };

        await _unitOfWork.Deposits.AddAsync(deposit);
        await _unitOfWork.SaveChangesAsync();

        return new PaymentIntentDTO
        {
            PaymentIntentId = intent.Id,
            ClientSecret = intent.ClientSecret!
        };
    }

    public async Task HandleStripeWebhookAsync(string requestBody, string signature)
    {
        var endpointSecret = _config["StripeSettings:WebhookSecret"];
        var stripeEvent = EventUtility.ConstructEvent(requestBody, signature, endpointSecret);

        if (stripeEvent.Type == "payment_intent.succeeded")
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            var deposit = await _unitOfWork.Deposits.GetByPaymentIntentIdAsync(paymentIntent!.Id);

            if (deposit != null)
            {
                deposit.IsPaid = true;
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }

    public async Task<bool> HasUserPaidDepositAsync(Guid userId, Guid auctionId)
    {
        return await _unitOfWork.Deposits.HasPaidDepositeAsync(userId, auctionId);
    }

    public async Task<bool> ConfirmDepositIntentAsync(string paymentIntentId)
    {
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

        var service = new PaymentIntentService();
        var options = new PaymentIntentConfirmOptions
        {
            PaymentMethod = "pm_card_visa" 
        };

        var intent = await service.ConfirmAsync(paymentIntentId, options);

        if (intent.Status == "succeeded")
        {
            var deposit = await _unitOfWork.Deposits.GetByPaymentIntentIdAsync(paymentIntentId);
            if (deposit != null && !deposit.IsPaid)
            {
                deposit.IsPaid = true;
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }

        return false;
    }

}
