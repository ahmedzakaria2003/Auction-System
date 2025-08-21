using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Specification
{
    public class UserSpecificationForCount : BaseSpecification<ApplicationUser>
    {
        public UserSpecificationForCount(UserQueryParams specParams)
            : base(u =>
                // البحث بالاسم أو الإيميل
                (string.IsNullOrEmpty(specParams.Search) ||
                    u.FullName.ToLower().Contains(specParams.Search.ToLower()) ||
                    u.Email.ToLower().Contains(specParams.Search.ToLower()))

                && (string.IsNullOrEmpty(specParams.UserType) ||
                    u.UserType.ToLower() == specParams.UserType.ToLower())

                && (string.IsNullOrEmpty(specParams.Status) ||
                    (specParams.Status.ToLower() == "active" && u.IsBanned == false) ||
                    (specParams.Status.ToLower() == "banned" && u.IsBanned == true))
            )
        {
        }
    }
}