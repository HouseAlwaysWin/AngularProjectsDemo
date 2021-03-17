using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.QuerySpecs;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Core.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T :class
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
           this._context = context; 
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task<int> CountAsync(IQuerySpec<T> spec)
        {
           return await ApplyQuerySpec(spec).CountAsync();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntityWithSpec(IQuerySpec<T> spec)
        {
            return await ApplyQuerySpec(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(IQuerySpec<T> spec)
        {
            return await ApplyQuerySpec(spec).ToListAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<T> ApplyQuerySpec(IQuerySpec<T> spec){
            var query = _context.Set<T>().AsQueryable();

            if(spec.Where != null){
                query = query.Where(spec.Where);
            }

            if(spec.OrderBy != null){
                query = query.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending != null){
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if(spec.IsPagingEnabled){
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (current,include) => current.Include(include));

            return query;
        }
    }
}