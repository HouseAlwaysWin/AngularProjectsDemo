using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;
using LinqToDB.Linq;

namespace BackendApi.Core.Data.Repositories.Interfaces
{
    public interface IBaseRepository<TContext> : IDisposable
    {
        Task AddAsync<TEntity>(TEntity entity) where TEntity:class,IBaseEntity;
        Task BulkAddAsync<TEntity>(IEnumerable<TEntity> entities)where TEntity:class,IBaseEntity;
        Task<IList<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null) where TEntity:class,IBaseEntity;
        Task<PagedList<TEntity>> GetAllPagedAsync<TEntity>(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null) where TEntity:class,IBaseEntity;
        Task<TEntity> GetByAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)where TEntity:class,IBaseEntity;
        Task<TEntity> GetByIdAsync<TEntity>(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)where TEntity:class,IBaseEntity;
        void Remove<TEntity>(TEntity entity)where TEntity:class,IBaseEntity;
        Task RemoveAsync<TEntity>(TEntity entity)where TEntity:class,IBaseEntity;
        Task RemoveByIdAsync<TEntity>(int id) where TEntity:class,IBaseEntity;
        void Update<TEntity>(TEntity entity)where TEntity:class,IBaseEntity;
        Task UpdateAsync<TEntity>(TEntity entity, 
                    Func<IQueryable<TEntity>,IUpdatable<TEntity>> func)where TEntity:class,IBaseEntity;
        Task UpdateAsync<TEntity>(TEntity entity, Dictionary<string,object> props) where TEntity:class,IBaseEntity;
        Task<bool> CompleteAsync();  
    }
}