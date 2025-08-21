using AuctionSystem.Application.DTOS;
using AuctionSystem.Application.DTOS.CategoryProfile;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesForDropdownAsync();

        Task<PaginatedResult<CategoryDto>> GetAllCategoriesAsync(AuctionQueryParamsDto paramsDto );
        Task<IEnumerable<CategoryWithAuctionsDto>> GetAllCategoriesWithAuctionsAsync();

        Task<PaginatedResult<CategoryWithAuctionsDto>> GetCategoryWithAuctionsAsync(AuctionQueryParamsDto paramsDto, Guid categoryId);

        Task<Guid> CreateCategoryAsync(CreateCategoryDto dto);

        Task<bool> UpdateCategoryAsync(Guid categoryId, UpdateCategoryDto dto);

        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }

}
