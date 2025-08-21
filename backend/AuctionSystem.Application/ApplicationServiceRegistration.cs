using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Infrastructure.Services;
using AuctionSystem.Application.Services.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionSystem.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBidService, BidService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IPayementService, PaymentService>();
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddScoped<IDepositService, DepositService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ISellerService, SellerService>();
        services.AddScoped<ISellerFeedbackService, SellerFeedbackService>();
        services.AddAutoMapper(typeof(AssemblyReference).Assembly);
        services.AddHostedService<WinnerBackgroundService>();

        return services;
    }
}
