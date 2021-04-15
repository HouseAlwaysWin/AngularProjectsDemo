using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Core.Data.Repositories.Interfaces;

namespace EcommerceApi.Core.Data.Repositories
{

    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
    {

        private readonly IRedisCachedService _redisCachedService;
        private readonly StoreContext _context;
        public EntityRepository(
            StoreContext context,
            IRedisCachedService redisCachedService)
        {
            this._redisCachedService = redisCachedService;
            this._context = context;
        }

        private async Task<IList<TEntity>> GetAllNoCachedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.ToListAsync();
        }

        private async Task<PagedList<TEntity>> GetAllPagedNoCachedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;

            var total = await query.CountAsync();

            var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<TEntity>(data, total);
        }


        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null)
        {
            if (useCached)
            {
                var methodName = nameof(GetAllAsync);
                if (string.IsNullOrEmpty(key))
                {
                    key = _redisCachedService.CreateKey<TEntity>(methodName);
                }


                var result = await _redisCachedService.GetAndSetAsync<IList<TEntity>>(key, async () =>
                {
                    return await GetAllNoCachedAsync(func);
                });
                return result;
            }
            return await GetAllNoCachedAsync(func);
        }


        public virtual async Task<PagedList<TEntity>> GetAllPagedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null)
        {

            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;

            var total = await query.CountAsync();

            var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<TEntity>(data, total);

        }


        public virtual async Task<TEntity> GetByAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.FirstOrDefaultAsync();
        }


        public virtual async Task<TEntity> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }


        public virtual async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _context.Set<TEntity>().AddAsync(entity);
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


        public virtual void Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Set<TEntity>().Remove(entity);
        }


    }
}