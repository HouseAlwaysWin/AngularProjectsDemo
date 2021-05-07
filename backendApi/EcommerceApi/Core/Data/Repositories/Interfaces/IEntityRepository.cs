using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.Repositories.Interfaces
{
    public interface IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null);
        Task<PagedList<TEntity>> GetAllPagedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, bool useCached = false, string key = null);
        Task<TEntity> GetByAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
        Task<TEntity> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}