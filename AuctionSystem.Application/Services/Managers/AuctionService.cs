using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.AuctionDTO;
using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Application.Specification;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Shared;
using AuctionSystem.Shared.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Managers
{
    public class AuctionService : IAuctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IHubContext<AuctionHub> _hubContext;  

        public AuctionService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, IHubContext<AuctionHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _hubContext = hubContext; 
        }

        // دالة لإنشاء مزاد جديد
        public async Task<Guid> CreateAuctionAsync(CreateAuctionDto dto, Guid userId)
        {
            var seller = await _unitOfWork.Users.GetByIdAsync(userId);
            if (seller == null || seller.IsBanned)
                throw new UnauthorizedException("You are banned from creating auctions.");

            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new NotFoundException($"Category With ID {dto.CategoryId} not found");

            var auction = _mapper.Map<Auction>(dto);
            auction.CreatedById = userId;

            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var image in dto.Images)
                {
                    var fileName = await _fileService.SaveFileAsync(image, "images");
                    auction.Images.Add(new AuctionImage { FileName = fileName });
                }
            }

            await _unitOfWork.Auctions.AddAsync(auction);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.Group(auction.Id.ToString()).
                SendAsync("AuctionCreated", auction.Id, auction.Title);

            return auction.Id;
        }

        public async Task<bool> UpdateAuctionAsync(Guid auctionId, UpdateAuctionDto dto, Guid userId)
        {
            var seller = await _unitOfWork.Users.GetByIdAsync(userId);
            if (seller == null || seller.IsBanned)
                throw new UnauthorizedException("You are banned from performing this action.");

            var auction = await _unitOfWork.Auctions.GetAuctionWithImagesAsync(auctionId);

            if (auction is null)
                throw new NotFoundException($"Auction with ID {auctionId} not found.");
            if (auction.CreatedById != userId)
                throw new UnauthorizedException
                    ($"You are not allowed to update this auction with ID {auctionId}.");

            if (auction.StartTime <= DateTime.UtcNow)
            {
                if (dto.EndTime != auction.EndTime || dto.StartingPrice != auction.StartingPrice)
                {
                    throw new BadRequestException("You cannot modify StartPrice or EndDate after the auction has started.");
                }
            }

            _mapper.Map(dto, auction);

            if (dto.Images is not null && dto.Images.Count > 0)
            {
                foreach (var oldImage in auction.Images)
                {
                    await _fileService.DeleteFileAsync(oldImage.FileName); // file path
                }

                auction.Images.Clear();

                foreach (var formFile in dto.Images)
                {
                    var imageUrl = await _fileService.SaveFileAsync(formFile, "images");

                    auction.Images.Add(new AuctionImage
                    {
                        FileName = imageUrl,
                        AuctionId = auction.Id
                    });
                }
            }

            await _hubContext.Clients.Group(auction.Id.ToString()).
                SendAsync("AuctionUpdated", auction.Id, auction.Title);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAuctionAsync(Guid auctionId, Guid userId)
        {
            var seller = await _unitOfWork.Users.GetByIdAsync(userId);
            if (seller == null || seller.IsBanned)
                throw new UnauthorizedException("You are banned from performing this action.");

            var auction = await _unitOfWork.Auctions.GetAuctionWithImagesAsync(auctionId);

            if (auction is null)
                throw new NotFoundException($"Auction with ID {auctionId} not found.");
            if (auction.Bids.Count > 0)
                throw new BadRequestException($"Cannot delete auction {auctionId} with bids.");

            if (auction.Images != null && auction.Images.Any())
            {
                foreach (var image in auction.Images)
                {
                    await _fileService.DeleteFileAsync(Path.Combine("wwwroot", "images", image.FileName));
                }
            }

            if (auction.CreatedById != userId)
            {
                throw new UnauthorizedException
                    ($"You are not allowed to Delete this auction with ID {auctionId}.");
            }

            await _unitOfWork.Auctions.DeleteAsync(auction);

            await _hubContext.Clients.Group(auction.Id.ToString()).
                SendAsync("AuctionDeleted", auction.Id,auction.Title);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<AuctionListDto>> GetAllAuctionsAsync()
        {
            var auctions = await _unitOfWork.Auctions.GetAllAuctionsAsync();
            if (auctions == null || !auctions.Any())
                throw new NotFoundException("No auctions found.");  
            return _mapper.Map<IEnumerable<AuctionListDto>>(auctions);
        }

        public async Task<IEnumerable<AuctionListDto>> GetActiveAuctionsAsync(AuctionQueryParams queryParams)
        {
            var spec = new AuctionWithFiltersSpecification(queryParams);
            var auctions = await _unitOfWork.Auctions.ListAsync(spec);

            if (auctions == null || !auctions.Any())
                throw new NotFoundException("No active auctions found.");

            return _mapper.Map<IEnumerable<AuctionListDto>>(auctions);
        }

        public async Task<AuctionDetailsDto?> GetAuctionDetailsAsync(Guid auctionId)
        {
            var auction = await _unitOfWork.Auctions.GetAuctionWithDetailsAsync(auctionId);
            if (auction == null)
                throw new NotFoundException($"Auction with ID {auctionId} not found.");

            return _mapper.Map<AuctionDetailsDto>(auction);
        }

        public async Task<IEnumerable<AuctionListDto>> GetAuctionsByCreatorAsync(AuctionQueryParams queryParams, Guid userId)
        {
            var spec = new AuctionByCreatorSpecification(queryParams, userId);
            var auctions = await _unitOfWork.Auctions.ListAsync(spec);
            if (auctions == null || !auctions.Any())
                throw new NotFoundException($"No auctions found for user with ID {userId}.");
            return _mapper.Map<IEnumerable<AuctionListDto>>(auctions);
        }

        public async Task<bool> CancelAuctionAsync(Guid auctionId)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId);
            if (auction == null)
                throw new NotFoundException($"Auction with ID {auctionId} not found.");

            auction.IsCanceled = true;
             await _unitOfWork.SaveChangesAsync();
            await _hubContext.Clients.Group(auctionId.ToString()).
                SendAsync("AuctionCanceled", auctionId,auction.Title);
            return true;
        }

        public async Task<WinnerDto> DeclareWinnerAsync(Guid auctionId)
        {
            var highestBid = await _unitOfWork.Bids.GetHighestBidForAuctionAsync(auctionId);

            if (highestBid == null || highestBid.Auction == null)
                throw new NotFoundException($"No bids found for auction with ID {auctionId}.");

            var auction = highestBid.Auction;
            if (auction.IsCanceled)
                throw new BadRequestException($"Auction with ID {auctionId} is canceled, cannot declare a winner.");

            if (auction.EndTime > DateTime.UtcNow)
                throw new BadRequestException($"Auction with ID {auctionId} is still ongoing, cannot declare a winner.");

            auction.WinnerId = highestBid.BidderId;
            auction.FinalPrice = highestBid.Amount;
            await _unitOfWork.Auctions.UpdateAsync(auction);
            await _unitOfWork.SaveChangesAsync();

            var Winner = highestBid.Bidder;
            if (Winner == null)
                throw new NotFoundException("Winner details not found");


            await _hubContext.Clients.Group(auctionId.ToString()).
                SendAsync("AuctionWinnerDeclared", Winner.FullName, highestBid.Amount);

            return new WinnerDto
            {
                AuctionTitle = auction.Title,
                WinnerName = Winner.FullName,
                WinningAmount = highestBid.Amount,
                Email = Winner.Email,
                PhoneNumber = Winner.PhoneNumber
            };
        }

        public async Task<IEnumerable<Auction>> GetAuctionsThatEndedWithoutWinnerAsync()
        {
            return await _unitOfWork.Auctions.GetAuctionsThatEndedWithoutWinnerAsync();
        }

        public async Task<IEnumerable<AuctionListDto>> GetHotAuctionsAsync(int take = 10)
        {
            var activeAuctions = await _unitOfWork.Auctions.GetHotAuctionsAsync(take);

            var hotAuctions = activeAuctions
                .OrderByDescending(a => a.Bids.Count)
                .Take(take);

            return _mapper.Map<IEnumerable<AuctionListDto>>(hotAuctions);
        }

        public async Task<IEnumerable<AuctionListDto>> GetRecommendedAuctionsForBidderAsync(Guid userId)
        {
            var recommendedAuctions = await _unitOfWork.Auctions.GetRecommendedAuctionsForBidderAsync(userId);

            return _mapper.Map<IEnumerable<AuctionListDto>>(recommendedAuctions);
        }
    }
}
