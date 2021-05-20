using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.Repositories
{
    public class IdentityRepository<TEntity> : BaseRepository<TEntity,StoreContext>,IIdentityRepository<TEntity> where TEntity : BaseEntity
    {
        
    }
}