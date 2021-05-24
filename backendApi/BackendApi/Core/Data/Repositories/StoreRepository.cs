using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Data.Repositories
{
    public class StoreRepository<TEntity> : BaseRepository<TEntity,StoreContext>,IStoreRepository<TEntity> where TEntity : BaseEntity
    {

    }
}