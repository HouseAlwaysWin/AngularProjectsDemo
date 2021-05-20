using System;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.Repositories.Interfaces
{
    public interface IUowBase:IDisposable
    {
        IBaseRepository<TEntity> EntityRepo<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();  
    }
}