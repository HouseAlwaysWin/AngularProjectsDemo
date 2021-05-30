using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities.Identity;
using BackendApi.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            this._userRepo = userRepo;
            this._mapper = mapper;
        }

        public async Task<List<UserFriendMapDto>> GetUserFriendsAsync(int userId)
        {
            var friendsMap = await _userRepo.GetAllAsync<UserFriend>(query =>
               query.Where(u => u.AppUserId == userId)
                   .Include(u => u.Friend)
                   .ThenInclude(u => u.UserInfo));

            var friends = _mapper.Map<List<UserFriend>, List<UserFriendMapDto>>(friendsMap.ToList());
            return friends;
        }

        public async Task<AppUser> GetUserByEmail(string email){
            var user = await _userRepo.GetByAsync<AppUser>(query => query.Where(u=> u.Email == email));
            return user;
        }

    }
}