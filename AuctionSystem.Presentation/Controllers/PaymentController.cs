using Auction_System.Controllers;
using AuctionSystem.Application.DTOS.PaymentDTO;
using AuctionSystem.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


public class PaymentController(IServiceManager _serviceManager) : ApiBaseController 
{
    [Authorize(Roles = "Bidder")]

    [HttpPost("create-intent/{auctionId}")]
    public async Task<ActionResult<PaymentIntentDTO>> CreatePaymentIntent(Guid auctionId)
    {
        try
        {
            var result = await _serviceManager.PayementService.CreatePaymentIntentForAuctionAsync(auctionId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        Console.WriteLine(" Webhook endpoint hit");

        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        if (!Request.Headers.TryGetValue("Stripe-Signature", out var stripeSignature))
        {
            Console.WriteLine(" Stripe-Signature header is missing.");
            return BadRequest(new { message = "Missing Stripe-Signature header" });
        }

        try
        {
            Console.WriteLine(" Passing to service...");
            await _serviceManager.PayementService.HandleStripeWebhookAsync(json, stripeSignature);
            Console.WriteLine(" Webhook handled successfully.");
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(" Exception: " + ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("confirm-intent/{paymentIntentId}")]
    public async Task<ActionResult> ConfirmIntent(string paymentIntentId)
    {
        try
        {
            var isSuccess = await _serviceManager.PayementService.ConfirmPaymentIntentAsync(paymentIntentId);

            return isSuccess
                ? Ok(new { message = " Payment confirmed successfully" })
                : BadRequest(new { message = " Payment failed or not succeeded" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


}
