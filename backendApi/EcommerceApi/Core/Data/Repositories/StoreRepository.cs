using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.Repositories
{
    public class StoreRepository<TEntity> : BaseRepository<TEntity,StoreContext>,IStoreRepository<TEntity> where TEntity : BaseEntity
    {

    }
}