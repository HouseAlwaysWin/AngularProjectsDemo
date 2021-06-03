using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;
using LinqToDB.Linq;

namespace BackendApi.Core.Data.Repositories.Interfaces
{
    public interface IBaseRepository : IDisposable
    {
        Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
        Task BulkAddAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        Task<bool> CompleteAsync();
        Task<IList<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null) where TEntity : class;
        Task<PagedList<TEntity>> GetAllPagedAsync<TEntity>(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null) where TEntity : class;
        Task<TEntity> GetByAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;
        Task RemoveAsync<TEntity>(Expression<Func<TEntity, bool>> IdentityPredicate) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        Task UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> IdentityPredicate, Func<TEntity, IUpdatable<TEntity>> func) where TEntity : class;
        Task UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> IdentityPredicate, Dictionary<string, object> props) where TEntity : class;

        bool HasChanges();
    }
}