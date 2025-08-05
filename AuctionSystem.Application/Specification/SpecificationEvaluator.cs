using AuctionSystem.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AuctionSystem.Application.Specification
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Apply normal expression-based includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Apply string-based includes (used for ThenInclude)
            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));


            return query;
        }
    }
}
