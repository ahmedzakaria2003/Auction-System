using AuctionSystem.Application.DTOS.PaymentDTO;
using AuctionSystem.Application.Services.Contracts;
using Auction_System.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction_System.Controllers
{
  
    [Authorize(Roles = "Bidder")]
   
    public class DepositController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpPost("create-intent/{auctionId:guid}")]
        public async Task<ActionResult<PaymentIntentDTO>> CreateDepositIntent(Guid auctionId)
        {
            try
            {
                var userId = User.GetUserId();
                var result = await _serviceManager.DepositService.CreateDepositIntentAsync(userId.Value , auctionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<ActionResult> StripeWebhook()
        {
            Console.WriteLine(" Deposit webhook hit");
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            if (!Request.Headers.TryGetValue("Stripe-Signature", out var stripeSignature))
            {
                Console.WriteLine(" Stripe-Signature header missing.");
                return BadRequest(new { message = "Missing Stripe-Signature header" });
            }

            try
            {
                await _serviceManager.DepositService.HandleStripeWebhookAsync(json, stripeSignature);
                Console.WriteLine(" Deposit webhook handled successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Exception: " + ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }





        [HttpGet("has-paid")]
        public async Task<ActionResult> HasPaidDeposit(Guid auctionId)
        {
            try
            {
                var userId = User.GetUserId();
                var hasPaid = await _serviceManager.DepositService.HasUserPaidDepositAsync(userId.Value, auctionId);
                return Ok(new { hasPaid });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("confirm-intent/{paymentIntentId}")]
        public async Task<ActionResult> ConfirmDepositIntent(string paymentIntentId)
        {
            try
            {
                var success = await _serviceManager.DepositService.ConfirmDepositIntentAsync(paymentIntentId);

                return success
                    ? Ok(new { message = " Deposit confirmed successfully" })
                    : BadRequest(new { message = " Deposit failed or not succeeded" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
