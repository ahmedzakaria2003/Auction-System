using AuctionSystem.Domain.Entities;
using AuctionSystem.Shared;

namespace AuctionSystem.Application.Contracts
{
    public interface ICategoryRepository : IGenericRepository<Category> 
    {


        Task<IEnumerable<Category>> GetAllCategoriesWithAuctionsAsync();

    }
}