// using System;
// using System.Collections;
// using System.Threading.Tasks;
// using EcommerceApi.Core.Data.Repositories.Interfaces;
// using EcommerceApi.Core.Models.Entities;

// namespace EcommerceApi.Core.Data.Repositories
// {
//     public class UnitOfWork : IUnitOfWork
//     {
//        private readonly StoreContext _context;
//         private Hashtable _repositories;
//         public UnitOfWork(StoreContext context)
//         {
//            this._context = context; 
//         }
//         public async Task<int> CompleteAsync()
//         {
//             return await _context.SaveChangesAsync();
//         }

//         public void Dispose()
//         {
//             this._context.Dispose();
//         }

//          public IEntityRepository<TEntity> EntityRepository<TEntity>() where TEntity : BaseEntity 
//         {
//             if(_repositories == null) _repositories = new Hashtable();

//             var type = typeof(TEntity).Name;

//             try{
//                 if(!_repositories.ContainsKey(type)){
//                     var repositoryType = typeof(BaseRepository<,>);
//                     var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)),_context);
//                     _repositories.Add(type,repositoryInstance);
//                 }
//             }catch(Exception ex){
//                 System.Console.WriteLine(ex);
//             }

//             return (IEntityRepository<TEntity>) _repositories[type];
//         }
//     }
// }