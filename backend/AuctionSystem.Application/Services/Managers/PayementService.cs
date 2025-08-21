using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.PaymentDTO;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Stripe;


public class PaymentService(IConfiguration config, IUnitOfWork _unitOfWork) : IPayementService
{
  

    public async Task<PaymentIntentDTO> CreatePaymentIntentForAuctionAsync(Guid auctionId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];


        var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId)
            ?? throw new NotFoundException("Auction not found");

        if (auction.IsPaid)
            throw new BadRequestException("Auction is already paid");

        var service = new PaymentIntentService();
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(auction.FinalPrice*10),
            Currency = "usd",
            PaymentMethodTypes = ["card"]
        };

        var intent = await service.CreateAsync(options);

        auction.PaymentIntentId = intent.Id;
        await _unitOfWork.SaveChangesAsync();

        return new PaymentIntentDTO
        {
            ClientSecret = intent.ClientSecret!,
            PaymentIntentId = intent.Id
        };
    }

    public async Task HandleStripeWebhookAsync(string requestBody, string signature)
    {
        var endpointSecret = config["StripeSettings:WebhookSecret"];
        var stripeEvent = EventUtility.ConstructEvent(requestBody, signature, endpointSecret);

        Console.WriteLine($" Stripe Webhook Event Received: {stripeEvent.Type}");

        if (stripeEvent.Type == "payment_intent.succeeded")
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            Console.WriteLine($" paymentIntent Id: {paymentIntent?.Id}");

            var auction = await _unitOfWork.Auctions.GetByPaymentIntentIdAsync(paymentIntent!.Id);

            if (auction == null)
            {
                Console.WriteLine(" No auction found with this paymentIntent.");
                return;
            }

            auction.IsPaid = true;
            
            var result = await _unitOfWork.SaveChangesAsync();
            Console.WriteLine($"Auction updated: {auction.Id}, SaveChanges result: {result}");
        }
    }


    public async Task<bool> ConfirmPaymentIntentAsync(string paymentIntentId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

        var service = new PaymentIntentService();

   

        var intent = await service.GetAsync(paymentIntentId);


        if (intent.Status == "succeeded")
        {
            var Pay = await _unitOfWork.Auctions.GetByPaymentIntentIdAsync(paymentIntentId);
            if (Pay != null && !Pay.IsPaid)
            {
                Pay.IsPaid = true;
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }

        return false;
    }


}
