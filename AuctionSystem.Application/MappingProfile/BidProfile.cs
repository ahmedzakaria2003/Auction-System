using AuctionSystem.Application.DTOS.BidDTO;
using AuctionSystem.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.MappingProfile
{
   public class BidProfile : Profile
    {
        public BidProfile()
        {
            CreateMap<Bid, BidDto>()
           .ForMember(dest => dest.AuctionId, opt => opt.MapFrom(src => src.AuctionId))
           .ForMember(dest => dest.BidderId, opt => opt.MapFrom(src => src.BidderId))
           .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
           .ForMember(dest => dest.BidderName, opt => opt.MapFrom(src => src.Bidder.UserName)) 
           .ForMember(dest => dest.BidTime, opt => opt.MapFrom(src => src.BidTime));

            CreateMap<AddBidDto, Bid>();


        }

    }
}
