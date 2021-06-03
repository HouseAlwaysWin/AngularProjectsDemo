using System.Collections.Generic;
using System.Threading.Tasks;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserFriendMapDto>> GetUserFriendsByEmailAsync(string email);
        Task<AppUserShortDto> GetUserByEmailAsync(string email);
        Task<AppUserShortDto> GetUserByPublicIdAsync(string publicId);
        Task<AppUserDto> GetUserDetailByEmailAsync(string email);
    }
}