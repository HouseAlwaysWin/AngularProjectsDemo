using System.Collections.Generic;
using System.Threading.Tasks;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserFriendMapDto>> GetUserFriendsAsync(int userId);
        Task<AppUser> GetUserByEmail(string email);
    }
}