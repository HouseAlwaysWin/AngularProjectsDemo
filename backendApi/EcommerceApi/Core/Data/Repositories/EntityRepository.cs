// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using EcommerceApi.Core.Models.Entities;
// using EcommerceApi.Core.Services;
// using EcommerceApi.Core.Services.Interfaces;
// using Microsoft.EntityFrameworkCore;
// using EcommerceApi.Core.Data.Repositories.Interfaces;
// using LinqToDB.EntityFrameworkCore;
// using LinqToDB;
// using LinqToDB.Data;
// using LinqToDB.Tools;

// namespace EcommerceApi.Core.Data.Repositories
// {

//     public class EntityRepository<TEntity,TContext> : IEntityRepository<TEntity> where TEntity : BaseEntity where TContext: DbContext {
//         private readonly TContext _context;
//         public EntityRepository()
//         {
//             LinqToDBForEFTools.Initialize();
//         }
//         public EntityRepository(TContext context)
//         {
//             this._context = context;
//             LinqToDBForEFTools.Initialize();
//         }

//         private async Task<PagedList<TEntity>> GetAllPagedNoCachedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
//         {
//             var query = _context.Set<TEntity>().AsQueryable();
//             query = func != null ? func(query) : query;

//             var total = await query.CountAsyncEF();

//             var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

//             return new PagedList<TEntity>(data, total);
//         }


//         public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null)
//         {
//             var query = _context.Set<TEntity>().AsQueryable();
//             query = func != null ? func(query) : query;
//             return await query.ToListAsyncEF();
//         }


//         public virtual async Task<PagedList<TEntity>> GetAllPagedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null)
//         {

//             var query = _context.Set<TEntity>().AsQueryable();
//             query = func != null ? func(query) : query;

//             var total = await query.CountAsyncEF();

//             var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

//             return new PagedList<TEntity>(data, total);

//         }


//         public virtual async Task<TEntity> GetByAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
//         {
//             var query = _context.Set<TEntity>().AsQueryable();
//             query = func != null ? func(query) : query;
//             return await query.FirstOrDefaultAsyncEF();
//         }


//         public virtual async Task<TEntity> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
//         {
//             var query = _context.Set<TEntity>().AsQueryable();
//             query = func != null ? func(query) : query;
//             return await query.FirstOrDefaultAsyncEF(e => e.Id == id);
//         }


//         public virtual async Task AddAsync(TEntity entity)
//         {
//             if (entity == null)
//             {
//                 throw new ArgumentNullException(nameof(entity));
//             }
//             await _context.Set<TEntity>().AddAsync(entity);
//         }

//         public virtual async Task BulkAddAsync(IEnumerable<TEntity> entities)
//         {
//             if (entities == null)
//             {
//                 throw new ArgumentNullException(nameof(entities));
//             }
//             await _context.BulkCopyAsync(new BulkCopyOptions(), entities);
//         }


//         public virtual void Update(TEntity entity)
//         {
//             if (entity == null)
//             {
//                 throw new ArgumentNullException(nameof(entity));
//             }

//             _context.Set<TEntity>().Attach(entity);
//             _context.Entry(entity).State = EntityState.Modified;
//         }


//         public virtual void Remove(TEntity entity)
//         {
//             if (entity == null)
//             {
//                 throw new ArgumentNullException(nameof(entity));
//             }
//             _context.Set<TEntity>().Remove(entity);
//         }

//     }

//     // public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
//     public class EntityRepository<TEntity> : EntityRepository<TEntity,StoreContext> where TEntity : BaseEntity
//     {

//         // private readonly StoreContext _context;
//         // public EntityRepository()
//         // {
//         //     LinqToDBForEFTools.Initialize();
//         // }
//         // public EntityRepository(StoreContext context)
//         // {
//         //     this._context = context;
//         //     LinqToDBForEFTools.Initialize();
//         // }

//         // private async Task<PagedList<TEntity>> GetAllPagedNoCachedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
//         // {
//         //     var query = _context.Set<TEntity>().AsQueryable();
//         //     query = func != null ? func(query) : query;

//         //     var total = await query.CountAsyncEF();

//         //     var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

//         //     return new PagedList<TEntity>(data, total);
//         // }


//         // public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null)
//         // {
//         //     var query = _context.Set<TEntity>().AsQueryable();
//         //     query = func != null ? func(query) : query;
//         //     return await query.ToListAsyncEF();
//         // }


//         // public virtual async Task<PagedList<TEntity>> GetAllPagedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null)
//         // {

//         //     var query = _context.Set<TEntity>().AsQueryable();
//         //     query = func != null ? func(query) : query;

//         //     var total = await query.CountAsyncEF();

//         //     var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

//         //     return new PagedList<TEntity>(data, total);

//         // }


//         // public virtual async Task<TEntity> GetByAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
//         // {
//         //     var query = _context.Set<TEntity>().AsQueryable();
//         //     query = func != null ? func(query) : query;
//         //     return await query.FirstOrDefaultAsyncEF();
//         // }


//         // public virtual async Task<TEntity> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
//         // {
//         //     var query = _context.Set<TEntity>().AsQueryable();
//         //     query = func != null ? func(query) : query;
//         //     return await query.FirstOrDefaultAsyncEF(e => e.Id == id);
//         // }


//         // public virtual async Task AddAsync(TEntity entity)
//         // {
//         //     if (entity == null)
//         //     {
//         //         throw new ArgumentNullException(nameof(entity));
//         //     }
//         //     await _context.Set<TEntity>().AddAsync(entity);
//         // }

//         // public virtual async Task BulkAddAsync(IEnumerable<TEntity> entities)
//         // {
//         //     if (entities == null)
//         //     {
//         //         throw new ArgumentNullException(nameof(entities));
//         //     }
//         //     await _context.BulkCopyAsync(new BulkCopyOptions(), entities);
//         // }


//         // public virtual void Update(TEntity entity)
//         // {
//         //     if (entity == null)
//         //     {
//         //         throw new ArgumentNullException(nameof(entity));
//         //     }

//         //     _context.Set<TEntity>().Attach(entity);
//         //     _context.Entry(entity).State = EntityState.Modified;
//         // }


//         // public virtual void Remove(TEntity entity)
//         // {
//         //     if (entity == null)
//         //     {
//         //         throw new ArgumentNullException(nameof(entity));
//         //     }
//         //     _context.Set<TEntity>().Remove(entity);
//         // }


//     }
// }