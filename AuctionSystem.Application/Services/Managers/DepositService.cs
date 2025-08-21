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
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(requestBody, signature, endpointSecret);
            Console.WriteLine($"Webhook received: {stripeEvent.Type}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Webhook signature verification failed: {ex.Message}");
            throw;
        }

        if (stripeEvent.Type == "payment_intent.succeeded")
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            Console.WriteLine($"PaymentIntent received: {paymentIntent?.Id}");

            var deposit = await _unitOfWork.Deposits.GetByPaymentIntentIdAsync(paymentIntent!.Id);

            if (deposit != null)
            {
                Console.WriteLine($"Found deposit! UserId={deposit.UserId}, AuctionId={deposit.AuctionId}, IsPaid={deposit.IsPaid}");
                deposit.IsPaid = true;
                await _unitOfWork.SaveChangesAsync();
                Console.WriteLine($"Deposit updated! IsPaid={deposit.IsPaid}");
            }
            else
            {
                Console.WriteLine("Deposit NOT found for this PaymentIntentId!");
            }
        }
        else
        {
            Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
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

        var intent = await service.GetAsync(paymentIntentId);

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
