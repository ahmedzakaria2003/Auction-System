using AuctionSystem.Application.Contracts;
using AuctionSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Data
{
    public class SeedData : IDataSeeding
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public SeedData(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            var roles = new[] { "Admin", "Seller", "Bidder" };

            foreach (var role in roles)
            {
                var exists = await _roleManager.RoleExistsAsync(role);
                if (!exists)
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var adminEmail = "admin@yahoo.com";
            var adminPassword = "Admin@123";

            if (await _userManager.FindByEmailAsync(adminEmail) is null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                };
                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var sellerEmail = "seller1@yahoo.com";
            if (await _userManager.FindByEmailAsync(sellerEmail) is null)
            {
                var seller = new ApplicationUser { UserName = "seller1", Email = sellerEmail };
                await _userManager.CreateAsync(seller, "Seller@123");
                await _userManager.AddToRoleAsync(seller, "Seller");
            }

            var bidderEmail = "bidder1@yahoo.com";
            if (await _userManager.FindByEmailAsync(bidderEmail) is null)
            {
                var bidder = new ApplicationUser { UserName = "bidder1", Email = bidderEmail };
                await _userManager.CreateAsync(bidder, "Bidder@123");
                await _userManager.AddToRoleAsync(bidder, "Bidder");
            }
        }
    }

}