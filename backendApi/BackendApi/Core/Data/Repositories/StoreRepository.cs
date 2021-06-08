using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Data.Store;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Data.Repositories
{
    public class StoreRepository : BaseRepository<StoreContext>,IStoreRepository
    {
        public StoreRepository(StoreContext context):base(context)
        {
            
        }
    }
}