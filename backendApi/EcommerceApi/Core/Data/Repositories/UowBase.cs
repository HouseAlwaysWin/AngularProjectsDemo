using System;
using System.Collections;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Core.Data.Repositories
{
    public abstract class UowBase<TContext> : IUowBase where TContext:DbContext
    {
        private readonly TContext _context;
        private Hashtable _repositories;
        public UowBase()
        {
        }
        public UowBase(TContext context)
        {
           this._context = context; 
        }
        public virtual async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._context.Dispose();
        }

        public virtual IBaseRepository<TEntity> EntityRepo<TEntity>() where TEntity : BaseEntity 
        {
            if(_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            try{
                if(!_repositories.ContainsKey(type)){
                    var repositoryType = typeof(BaseRepository<,>);
                    var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)),_context);
                    _repositories.Add(type,repositoryInstance);
                }
            }catch(Exception ex){
                System.Console.WriteLine(ex);
            }

            return (IBaseRepository<TEntity>) _repositories[type];
        }
    }
}