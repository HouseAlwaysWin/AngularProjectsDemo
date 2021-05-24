// using System;
// using System.Threading.Tasks;
// using BackendApi.Core.Models.Entities;

// namespace BackendApi.Core.Data.Repositories.Interfaces
// {
//     public interface IUowBase<TContext>:IDisposable
//     {
//         IBaseRepository<TContext> EntityRepo<TEntity,TRepository>();
//         Task<int> CompleteAsync();  
//     }
// }