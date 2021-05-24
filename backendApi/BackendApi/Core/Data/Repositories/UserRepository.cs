using BackendApi.Core.Data.Identity;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Data.Repositories
{
    public class UserRepository: BaseRepository<UserContext>,IUserRepository
    {
       public UserRepository(UserContext context):base(context)
       {
           
       } 
    }
}