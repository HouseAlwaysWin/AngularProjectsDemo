using System.Collections.Generic;
using System.Threading.Tasks;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserFriendMapDto>> GetUserFriendsDtoByEmailAsync(string email);
        Task<AppUserShortDto> GetUserDtoByIdAsync(int id);
        Task<AppUserShortDto> GetUserDtoByEmailAsync(string email);
        Task<AppUserShortDto> GetUserDtoByUserNameAsync(string userName);
        Task<AppUser> GetUserByUserNameAsync(string userName);
        Task<AppUserShortDto> GetUserDtoByPublicIdAsync(string publicId);
        Task<AppUserDto> GetUserDetailDtoByEmailAsync(string email);
        Task<PagedList<NotificationDto>> GetNotificationDtoAsync(int pageIndex,int pageSize,int userId);
    }
}