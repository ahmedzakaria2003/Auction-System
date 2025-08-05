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
    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.ToTable("Auctions");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.StartingPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.StartTime)
                .IsRequired();

            builder.Property(a => a.EndTime)
                .IsRequired()
                ;
            builder.HasOne(a => a.Category)
                .WithMany(c => c.Auctions)
                .HasForeignKey(a => a.CategoryId);

           builder.HasMany(a=> a.Images)
                .WithOne(i => i.Auction)
                .HasForeignKey(i => i.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.CreatedBy)
                .WithMany(U => U.CreatedAuctions)
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Winner)
                .WithMany(u => u.WonAuctions)
                .HasForeignKey(a => a.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Bids)
                .WithOne(b => b.Auction)
                .HasForeignKey(b => b.AuctionId);
                
        }
    }
}
