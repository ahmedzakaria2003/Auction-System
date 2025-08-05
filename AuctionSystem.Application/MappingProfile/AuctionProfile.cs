using AutoMapper;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Application.DTOS.AuctionProfile;

namespace AuctionSystem.Application.Mappings
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            // Auction <-> CreateAuctionDto
            CreateMap<CreateAuctionDto, Auction>()
       .ForMember(dest => dest.Images, opt => opt.Ignore()); 


            // Auction -> AuctionListDto
            CreateMap<Auction, AuctionListDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                  .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.CreatedBy.UserName))
                     .ForMember(dest => dest.BidsCount, opt => opt.MapFrom(src => src.Bids.Count()))
                .ForMember(dest => dest.ThumbnailImage,
         opt => opt.MapFrom(src => src.Images.Select(img => img.FileName).ToList()))
                .ForMember(dest => dest.AuctionStatus, opt => opt.MapFrom(src => src.Status.ToString()))
;



            // Auction -> AuctionDetailsDto
            CreateMap<Auction, AuctionDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.CreatedBy.UserName))

         .ForMember(dest => dest.ItemImageUrls,
             opt => opt.MapFrom(src => src.Images.Select(img => img.FileName).ToList()))

                .ForMember(dest => dest.WinnerName, opt => opt.
                MapFrom(src => src.Winner != null ? src.Winner.UserName : null))

                .ForMember(dest => dest.TotalBids, opt => opt.MapFrom(src => src.Bids.Count()))

                .ForMember(dest => dest.HighestBidAmount, opt => opt.MapFrom(src =>
                    src.Bids.Any() ? src.Bids.Max(b => b.Amount) : 0))

                .ForMember(dest => dest.HighestBidderName, opt => opt.MapFrom
                (src => src.Bids.OrderByDescending(b => b.Amount)
                .Select(b => b.Bidder.UserName).FirstOrDefault()));

        
            // Auction <-> UpdateAuctionDto
            CreateMap<UpdateAuctionDto, Auction>()
     .ForMember(dest => dest.Images, opt => opt.Ignore());



        }
    }
}
