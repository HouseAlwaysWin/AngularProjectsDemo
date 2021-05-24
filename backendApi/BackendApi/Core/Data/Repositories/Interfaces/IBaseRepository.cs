using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Data.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
                Task AddAsync(TEntity entity);
        Task BulkAddAsync(IEnumerable<TEntity> entities);
        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null);
        Task<PagedList<TEntity>> GetAllPagedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null);
        Task<TEntity> GetByAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
        Task<TEntity> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task RemoveByIdAsync(int id);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);
    }
}