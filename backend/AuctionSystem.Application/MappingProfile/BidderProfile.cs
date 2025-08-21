using AuctionSystem.Application.DTOS.ProfileDTO;
using AuctionSystem.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.MappingProfile
{
   public class BidderProfile : Profile
    {
        public BidderProfile()
        {
            CreateMap<Auction, AuctionWonDto>().ForMember(dest => dest.CategoryName, opt =>
            opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.WinningBidAmount, opt =>
                opt.MapFrom(src => src.FinalPrice))
                .ForMember(dest => dest.Images,
                   opt => opt.MapFrom(src => src.Images.Select(img => img.FileName).ToList()));

            CreateMap<Auction, ActiveBiddingDto>().ForMember(dest => dest.CategoryName, opt =>
            opt.MapFrom(src => src.Category.Name)).ForMember(dest => dest.Images,
                   opt => opt.MapFrom(src => src.Images.Select(img => img.FileName).ToList()))
            .ForMember(dest => dest.YourBid, opt =>
            opt.MapFrom((src, dest, destMember, context) =>
            {
                var userId = (Guid)context.Items["UserId"];
                return src.Bids.Where(b => b.BidderId == userId)
                .OrderByDescending(b=>b.BidTime).FirstOrDefault()?.Amount ?? 0;

            })).ForMember(dest => dest.CurrentHighestBid, opt =>
            opt.MapFrom(src => src.Bids.Max(b => b.Amount)));




        }
    }
}
