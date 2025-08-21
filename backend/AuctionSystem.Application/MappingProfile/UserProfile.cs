using AuctionSystem.Application.DTOS.AdminDTO;
using AuctionSystem.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.MappingProfile
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDto>().ForMember(des => des.UserId, opt =>
            opt.MapFrom(src => src.Id));
                
     
        }


    }
}
