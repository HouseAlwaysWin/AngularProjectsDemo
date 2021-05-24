// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using BackendApi.Core.Models.Entities;
// using Microsoft.EntityFrameworkCore;

// namespace BackendApi.Core.Data.Repositories.Interfaces
// {

//     public interface IEntityRepository<TEntity,TContext> where TEntity : BaseEntity where TContext :DbContext
//     {
//         Task AddAsync(TEntity entity);
//         Task BulkAddAsync(IEnumerable<TEntity> entities);
//         Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null);
//         Task<PagedList<TEntity>> GetAllPagedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null);
//         Task<TEntity> GetByAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
//         Task<TEntity> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
//         void Remove(TEntity entity);
//         void Update(TEntity entity);
//     }


//     public interface IEntityRepository<TEntity> where TEntity : BaseEntity
//     {
//         Task AddAsync(TEntity entity);
//         Task BulkAddAsync(IEnumerable<TEntity> entities);
//         Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null);
//         Task<PagedList<TEntity>> GetAllPagedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null);
//         Task<TEntity> GetByAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
//         Task<TEntity> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
//         void Remove(TEntity entity);
//         void Update(TEntity entity);
//     }
// }