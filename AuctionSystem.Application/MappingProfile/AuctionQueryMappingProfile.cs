using AutoMapper;
using AuctionSystem.Application.DTOS;
using AuctionSystem.Shared;

public class AuctionQueryMappingProfile : Profile
{
    public AuctionQueryMappingProfile()
    {
        CreateMap<AuctionQueryParamsDto, AuctionQueryParams>();
    }
}
