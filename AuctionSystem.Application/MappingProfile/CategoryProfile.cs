using AuctionSystem.Application.DTOS.CategoryProfile;
using AuctionSystem.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.MappingProfile
{
   public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryWithAuctionsDto>()
              .ForMember(dest => dest.PagedAuctions, opt => opt.MapFrom(src => src.Auctions));
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>();

        }

    }
}
