using AuctionSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Configurations
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
           builder.ToTable("Bids");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(b => b.BidTime)
                .IsRequired();

            builder.HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Bidder)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.BidderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
