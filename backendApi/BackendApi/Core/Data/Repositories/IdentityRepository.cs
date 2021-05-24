using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Data.Repositories
{
    public class IdentityRepository<TEntity> : BaseRepository<TEntity,StoreContext>,IIdentityRepository<TEntity> where TEntity : BaseEntity
    {
        
    }
}