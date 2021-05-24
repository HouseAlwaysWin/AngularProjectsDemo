// using System;
// using System.Threading.Tasks;
// using BackendApi.Core.Models.Entities;

// namespace BackendApi.Core.Data.Repositories.Interfaces
// {
//     public interface IUnitOfWork:IDisposable
//     {
//         IEntityRepository<TEntity> EntityRepository<TEntity>() where TEntity : BaseEntity;
//         Task<int> CompleteAsync(); 
//     }
// }