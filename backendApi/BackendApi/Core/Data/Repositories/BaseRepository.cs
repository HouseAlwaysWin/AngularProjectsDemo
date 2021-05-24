using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LinqToDB.Linq;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Data.Repositories
{

    public abstract class BaseRepository<TContext> : IBaseRepository<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        
        public BaseRepository(TContext context)
        {
            this._context = context;
            LinqToDBForEFTools.Initialize();
        }

        private async Task<PagedList<TEntity>> GetAllPagedNoCachedAsync<TEntity>(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
            where TEntity:class
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;

            var total = await query.CountAsyncEF();

            var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

            return new PagedList<TEntity>(data, total);
        }


        public virtual async Task<IList<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
            where TEntity: class,IBaseEntity
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.ToListAsyncEF();
        }


        public virtual async Task<PagedList<TEntity>> GetAllPagedAsync<TEntity>(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
             where TEntity: class,IBaseEntity
        {

            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;

            var total = await query.CountAsyncEF();

            var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

            return new PagedList<TEntity>(data, total);

        }


        public virtual async Task<TEntity> GetByAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
                where TEntity: class,IBaseEntity
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.FirstOrDefaultAsyncEF();
        }


        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null) 
                            where TEntity: class,IBaseEntity
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = func != null ? func(query) : query;
            return await query.FirstOrDefaultAsyncEF(e => e.Id == id);
        }


        public virtual async Task AddAsync<TEntity>(TEntity entity)
            where TEntity:class,IBaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task BulkAddAsync<TEntity>(IEnumerable<TEntity> entities)
                    where TEntity:class,IBaseEntity
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            await _context.BulkCopyAsync(new BulkCopyOptions(), entities);
        }


        public virtual void Update<TEntity>(TEntity entity)
                    where TEntity:class,IBaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

       public virtual async Task UpdateAsync<TEntity>(TEntity entity, 
                    Func<IQueryable<TEntity>,IUpdatable<TEntity>> func)
                where TEntity : class,IBaseEntity
        {
            var query = _context.GetTable<TEntity>()
                .Where(e => e.Id == entity.Id);
            await func(query).UpdateAsync();
        }

         public virtual async Task UpdateAsync<TEntity>(TEntity entity, 
                    Dictionary<string,object> props)
                where TEntity : class,IBaseEntity
        {
            var query = _context.GetTable<TEntity>()
                .Where(e => e.Id == entity.Id).AsUpdatable();

            foreach (var item in props)
            {
                query =query.Set(e => Sql.Property<TEntity>(e,item.Key),item.Value);
            }
            await query.UpdateAsync();
        }


        public virtual void Remove<TEntity>(TEntity entity)
                    where TEntity:class,IBaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual async Task RemoveAsync<TEntity>(TEntity entity)
                    where TEntity:class,IBaseEntity
        {

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var id = entity.Id;
            await _context.Set<TEntity>().DeleteWithOutputAsync(e => e.Id == id);
        }

        public virtual async Task RemoveByIdAsync<TEntity>(int id)
                    where TEntity:class,IBaseEntity
        {
            await _context.Set<TEntity>().DeleteWithOutputAsync(e => e.Id == id);
        }

        public virtual void Dispose()
        {
            this._context.Dispose();
        }

       public virtual async Task<int> CompleteAsync()
       {
         return await _context.SaveChangesAsync();
       }
    }
}