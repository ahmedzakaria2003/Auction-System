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
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryWithAuctionsDto>> GetAllCategoriesWithAuctionsAsync();

        Task<CategoryWithAuctionsDto> GetCategoryWithAuctionsAsync(AuctionQueryParams queryParams, Guid categoryId);

        Task<Guid> CreateCategoryAsync(CreateCategoryDto dto);

        Task<bool> UpdateCategoryAsync(Guid categoryId, UpdateCategoryDto dto);

        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }

}
