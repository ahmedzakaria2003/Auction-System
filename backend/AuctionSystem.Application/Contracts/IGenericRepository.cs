using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Contracts
{
  public interface IGenericRepository <TEntity> where TEntity : BaseEntity 
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        
        Task<TEntity> GetByIdAsync(Guid id);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity> DeleteAsync(TEntity entity);

        Task<TEntity> GetEntityWithSpecAsync(ISpecification<TEntity> spec);
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);
        Task<int> CountAsync(ISpecification<TEntity> spec);


    }
}
