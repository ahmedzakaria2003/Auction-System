using AuctionSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionSystem.Infrastructure.Configurations
{
    public class SellerFeedbackConfiguration : IEntityTypeConfiguration<SellerFeedback>
    {
        public void Configure(EntityTypeBuilder<SellerFeedback> builder)
        {
            builder.HasOne(f => f.Seller)
                .WithMany(u => u.SellerFeedbacks)
                .HasForeignKey(f => f.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Bidder)
                .WithMany()
                .HasForeignKey(f => f.BidderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Auction)
                .WithMany()
                .HasForeignKey(f => f.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}