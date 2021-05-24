// using System;
// using System.Collections;
// using System.Threading.Tasks;
// using BackendApi.Core.Data.Repositories.Interfaces;
// using BackendApi.Core.Models.Entities;
// using Microsoft.EntityFrameworkCore;

// namespace BackendApi.Core.Data.Repositories
// {
//     public abstract class UowBase<TContext> : IUowBase<TContext> where TContext:DbContext
//     {
//         private readonly TContext _context;
//         private Hashtable _repositories;
//         public UowBase(TContext context)
//         {
//            this._context = context; 
//         }
//         public virtual async Task<int> CompleteAsync()
//         {
//             return await _context.SaveChangesAsync();
//         }

//         public void Dispose()
//         {
//             this._context.Dispose();
//         }

//         public virtual IBaseRepository<TRepository> EntityRepo<TEntity,TRepository>() 
//         {
//             if(_repositories == null) _repositories = new Hashtable();

//             var type = typeof(TEntity).Name;

//             try{
//                 if(!_repositories.ContainsKey(type)){
//                     var repositoryType = typeof(TRepository);
//                     var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TContext)),_context);
//                     _repositories.Add(type,repositoryInstance);
//                 }
//             }catch(Exception ex){
//                 System.Console.WriteLine(ex);
//             }

//             return (IBaseRepository<TContext>) _repositories[type];
//         }
//     }
// }