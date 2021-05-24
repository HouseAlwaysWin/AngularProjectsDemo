using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Data.Repositories
{

    public abstract class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>  where TEntity : BaseEntity where TContext : DbContext
    {
        private readonly TContext _context;
        public BaseRepository()
        {
            LinqToDBForEFTools.Initialize();
        }
        public BaseRepository(TContext context)
        {
            this._context = context;
            LinqToDBForEFTools.Initialize();
        }

        private async Task<PagedList<TEntity>> GetAllPagedNoCachedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;

            var total = await query.CountAsyncEF();

            var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

            return new PagedList<TEntity>(data, total);
        }


        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.ToListAsyncEF();
        }


        public virtual async Task<PagedList<TEntity>> GetAllPagedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null)
        {

            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;

            var total = await query.CountAsyncEF();

            var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

            return new PagedList<TEntity>(data, total);

        }


        public virtual async Task<TEntity> GetByAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.FirstOrDefaultAsyncEF();
        }


        public virtual async Task<TEntity> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.FirstOrDefaultAsyncEF(e => e.Id == id);
        }


        public virtual async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task BulkAddAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            await _context.BulkCopyAsync(new BulkCopyOptions(), entities);
        }


        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().UpdateAsync(p => entity);
            _context.Entry(entity).State = EntityState.Modified;
        }


        public virtual void Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual async Task RemoveAsync(TEntity entity)
        {

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var id = entity.Id;
            await _context.Set<TEntity>().DeleteWithOutputAsync(e => e.Id == id);
        }

        public virtual async Task RemoveByIdAsync(int id)
        {
            await _context.Set<TEntity>().DeleteWithOutputAsync(e => e.Id == id);
        }
    }
}