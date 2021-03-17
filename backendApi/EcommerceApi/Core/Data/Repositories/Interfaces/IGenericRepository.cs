using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.QuerySpecs;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T :class
    {
         Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(IQuerySpec<T> spec);
        Task<IReadOnlyList<T>> ListAsync(IQuerySpec<T> spec);
        Task<int> CountAsync(IQuerySpec<T> spec);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}