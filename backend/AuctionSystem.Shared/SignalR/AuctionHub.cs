using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuctionSystem.Shared.SignalR
{
    public class AuctionHub : Hub
    {
        public async Task NotifyNewBid(string auctionId, decimal bidAmount, string bidderName)
        {
            await Clients.Group(auctionId).SendAsync("ReceiveBidNotification", bidAmount, bidderName);
        }

        public async Task JoinAuctionGroup(string auctionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, auctionId);
        }

        public async Task LeaveAuctionGroup(string auctionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, auctionId);
        }
    }
}