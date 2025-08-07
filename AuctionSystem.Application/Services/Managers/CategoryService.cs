using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.AuctionProfile;
using AuctionSystem.Application.DTOS.CategoryProfile;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Application.Specification;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Shared;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Managers
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            if (categories == null || !categories.Any())
                throw new NotFoundException("No categories found.");
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<PaginatedResult<CategoryWithAuctionsDto>> GetCategoryWithAuctionsAsync(AuctionQueryParams queryParams, Guid categoryId)
        {
            // Get the category first
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);

            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found.");

            // Apply filtering on Auctions manually
            var spec = new AuctionsByCategorySpecification(queryParams, categoryId);
            var countSpec = new AuctionsByCategorySpecificationForCount(queryParams, categoryId);
            var totalCount = await _unitOfWork.Auctions.CountAsync(countSpec);
            var filteredAuctions = await _unitOfWork.Auctions.ListAsync(spec);

            // Inject the filtered auctions into category manually
            category.Auctions = filteredAuctions.ToList();

            var auctionsDto =   _mapper.Map<IReadOnlyList<AuctionListDto>>(filteredAuctions);
           var pagedAuctions  = new PaginatedResult<AuctionListDto>
            {
                Data = auctionsDto,
                Count = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };

            var categoryDto = new CategoryWithAuctionsDto
            {
                Id = category.Id,
                Name = category.Name,
                PagedAuctions = pagedAuctions
            };
            return new PaginatedResult<CategoryWithAuctionsDto>
            {
                Data = new List<CategoryWithAuctionsDto> { categoryDto },
                Count = 1, // Since we are returning a single category
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }



        public async Task<Guid> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);

            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found."); 

            var auctions = await _unitOfWork.Auctions.GetAuctionsByCategoryAsync(categoryId);

            if (auctions.Any())
                throw new BadRequestException($"Cannot delete category with ID {categoryId} because it has auctions."); 

            await _unitOfWork.Categories.DeleteAsync(category);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCategoryAsync(Guid categoryId, UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);

            if (category is null)
                throw new NotFoundException($"Category with ID {categoryId} not found."); 
            _mapper.Map(dto, category);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<CategoryWithAuctionsDto>> GetAllCategoriesWithAuctionsAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllCategoriesWithAuctionsAsync();
            if (categories == null || !categories.Any())
                throw new NotFoundException("No categories with auctions found.");
            return  _mapper.Map<IEnumerable<CategoryWithAuctionsDto>>(categories);
        }
    }
}
