using AuctionSystem.Application.DTOS.SellerFeedbackDTO;
using AuctionSystem.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.MappingProfile
{
    public class SellerFeedbackProfile : Profile
    {
        public SellerFeedbackProfile()
        {
            CreateMap<SellerFeedback, SellerFeedbackDto>().ReverseMap();
        }
    }
}
