using System;
using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Data.Repositories.Interfaces
{
    public interface IUowBase:IDisposable
    {
        IBaseRepository<TEntity> EntityRepo<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();  
    }
}