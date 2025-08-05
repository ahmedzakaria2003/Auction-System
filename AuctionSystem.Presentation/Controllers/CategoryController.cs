using AuctionSystem.Application.DTOS.CategoryProfile;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction_System.Controllers
{
   
   
    public class CategoryController : ApiBaseController

    {
        private readonly IServiceManager _serviceManager;
        public CategoryController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            var result = await _serviceManager.CategoryService.GetAllCategoriesAsync();

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("with-active-auctions/{categoryId:guid}")]
        public async Task<ActionResult<IEnumerable<CategoryWithAuctionsDto>>> GetCategoryWithAuctionsAsync( [FromQuery] AuctionQueryParams queryParams , Guid categoryId)
        {
            var result = await _serviceManager.CategoryService.GetCategoryWithAuctionsAsync( queryParams ,categoryId);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateCategoryAsync([FromBody] CreateCategoryDto dto)
        {

            var category = await _serviceManager.CategoryService.CreateCategoryAsync(dto);
            return Ok(new { success = true, message = "Category created successfully.", id = category });

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId:guid}")]
        public async Task<ActionResult> DeleteCategoryAsync(Guid categoryId)
        {
            var result = await _serviceManager.CategoryService.DeleteCategoryAsync(categoryId);
            return Ok(new { success = true, message = "Category deleted successfully." });

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryId:guid}")]
        public async Task<ActionResult> UpdateCategoryAsync(Guid categoryId, [FromBody] UpdateCategoryDto dto)
        {
            var result = await _serviceManager.CategoryService.UpdateCategoryAsync(categoryId, dto);
            return Ok(new { success = true, message = "Category updated successfully." });
        }

        [AllowAnonymous]
        [HttpGet("with-auctions")]
        public async Task<ActionResult> GetCategoriesWithAuctions()
        {
            var result = await _serviceManager.CategoryService.GetAllCategoriesWithAuctionsAsync();
            return Ok(result);
        }


    }
}