using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Interfaces.Repositories;

namespace TPOS.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        // Implement Methods for batch operations
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public void DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }


        // Implement the GetAsync method with filter, orderBy, and include
        public async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool tracking = false)
        {
            var query = PrepareQueryInternal(tracking, filter, orderBy, include);
            return await query.ToListAsync();
        }

        // Implement the GetSingleAsync method with filter and include
        public async Task<T?> GetSingleAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool tracking = true)
        {
            var query = PrepareQueryInternal(tracking, filter, null, include);
            return await query.SingleOrDefaultAsync();
        }


        #region Helper
        private IQueryable<T> PrepareQueryInternal(bool disableTracking,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            // handle tracking
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            // handle include
            if (include != null)
            {
                query = include(query);
            }

            // handle filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //handle order by
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }
        #endregion
    }
}