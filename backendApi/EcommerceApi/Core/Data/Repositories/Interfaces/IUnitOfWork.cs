// using System;
// using System.Threading.Tasks;
// using EcommerceApi.Core.Models.Entities;

// namespace EcommerceApi.Core.Data.Repositories.Interfaces
// {
//     public interface IUnitOfWork:IDisposable
//     {
//         IEntityRepository<TEntity> EntityRepository<TEntity>() where TEntity : BaseEntity;
//         Task<int> CompleteAsync(); 
//     }
// }