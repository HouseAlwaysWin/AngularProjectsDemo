using System;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.Repositories.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        IEntityRepository<TEntity> EntityRepository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete(); 
    }
}