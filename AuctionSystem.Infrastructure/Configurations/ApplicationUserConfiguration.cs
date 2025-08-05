using AuctionSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionSystem.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(u => u.SellerFeedbacks)
                   .WithOne(f => f.Seller)
                   .HasForeignKey(f => f.SellerId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
