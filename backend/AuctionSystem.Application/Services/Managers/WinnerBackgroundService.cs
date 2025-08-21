using AuctionSystem.Application.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class WinnerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public WinnerBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();

            var auctions = await auctionService.GetAuctionsThatEndedWithoutWinnerAsync();

            foreach (var auction in auctions)
            {
                try
                {
                    await auctionService.DeclareWinnerAsync(auction.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[HostedService] Failed to declare winner for {auction.Id}: {ex.Message}");
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
