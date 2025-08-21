using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Linq;

namespace AuctionSystem.Application.Specification
{
    public class UserSpecification : BaseSpecification<ApplicationUser>
    {
        public UserSpecification(UserQueryParams specParams)
            : base(u =>
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

            ApplyPaging((specParams.PageNumber - 1) * specParams.PageSize, specParams.PageSize);
        }
    }
}
