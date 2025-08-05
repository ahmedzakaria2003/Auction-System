using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.Specification;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity :BaseEntity
    {
        private readonly AuctionDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(AuctionDbContext context )
        {
            _context = context ;
            _dbSet = _context.Set<TEntity>();   
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();

        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {

            return await _dbSet.FindAsync(id);


        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
           await   _dbSet.AddAsync(entity);
           return entity;

        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(entity);
        }

        public async Task<TEntity> GetEntityWithSpecAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_dbSet.AsQueryable(), spec);
        }
    }
}
