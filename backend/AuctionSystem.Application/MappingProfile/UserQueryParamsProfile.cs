using AuctionSystem.Application.DTOS;
using AuctionSystem.Shared;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.MappingProfile
{
    public class UserQueryParamsProfile : Profile
    {

        public UserQueryParamsProfile()
        {
            CreateMap<UserQueryParamsDto, UserQueryParams>();
        }
    }
}
