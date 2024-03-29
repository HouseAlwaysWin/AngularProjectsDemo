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

	public abstract class BaseRepository<TContext> : IBaseRepository where TContext : DbContext
	{
		private readonly TContext _context;

		public BaseRepository(TContext context)
		{
			this._context = context;
			LinqToDBForEFTools.Initialize();
		}


		public virtual async Task<int> GetTotalCountAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
		    where TEntity : class
		{
			var query = _context.Set<TEntity>().AsQueryable();
			query = func != null ? func(query) : query;
			return await query.CountAsyncEF();
		}


		public virtual async Task<IList<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
		    where TEntity : class
		{
			var query = _context.Set<TEntity>().AsQueryable();
			query = func != null ? func(query) : query;
			return await query.ToListAsyncEF();
		}


		public virtual async Task<PagedList<TEntity>> GetAllPagedAsync<TEntity>(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
		     where TEntity : class
		{

			var query = _context.Set<TEntity>().AsQueryable();
			query = func != null ? func(query) : query;

			var total = await query.CountAsyncEF();

			pageIndex = (pageIndex - 1) >= 0 ? pageIndex - 1 : 0;
			var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsyncEF();

			return new PagedList<TEntity>(data, total);

		}


		public virtual async Task<TEntity> GetByAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
			where TEntity : class
		{
			var query = _context.Set<TEntity>().AsQueryable();
			query = func != null ? func(query) : query;
			return await query.FirstOrDefaultAsyncEF();
		}


		public virtual async Task AddAsync<TEntity>(TEntity entity)
		    where TEntity : class
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}
			await _context.Set<TEntity>().AddAsync(entity);
		}

		public virtual async Task BulkAddAsync<TEntity>(IEnumerable<TEntity> entities)
			    where TEntity : class
		{
			if (entities == null)
			{
				throw new ArgumentNullException(nameof(entities));
			}
			await _context.BulkCopyAsync(new BulkCopyOptions(), entities);
		}


		public virtual void Update<TEntity>(TEntity entity)
			    where TEntity : class
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			_context.Set<TEntity>().Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public virtual async Task UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> IdentityPredicate,
			   Dictionary<Expression<Func<TEntity, object>>, object> props)
		       where TEntity : class
		{
			var query = _context.GetTable<TEntity>()
			    .Where(IdentityPredicate).AsUpdatable();

			foreach (var item in props)
			{
				var prop = (item.Key.Body as MemberExpression ?? ((UnaryExpression)item.Key.Body).Operand as MemberExpression).Member.Name;
				query = query.Set(e => Sql.Property<TEntity>(e, prop), item.Value);
			}
			await query.UpdateAsync();
		}


		public virtual void Remove<TEntity>(TEntity entity)
			    where TEntity : class
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}
			_context.Set<TEntity>().Remove(entity);
		}

		public virtual async Task RemoveAsync<TEntity>(Expression<Func<TEntity, bool>> IdentityPredicate)
			    where TEntity : class
		{
			await _context.Set<TEntity>().DeleteAsync(IdentityPredicate);
		}


		public virtual async Task<bool> CompleteAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public void Dispose()
		{
			_context.Dispose();
		}

		public bool HasChanges()
		{
			return _context.ChangeTracker.HasChanges();
		}
	}
}