using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
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





        public async Task<List<UserFriendMapDto>> GetUserFriendsDtoByEmailAsync(string email)
        {
            var friendsMap = await _userRepo.GetAllAsync<UserFriend>(query =>
               query.Where(u => u.AppUser.Email == email)
                   .Include(u => u.Friend)
                   .ThenInclude(u => u.UserInfo)
                   .Include(u => u.Friend)
                   .ThenInclude(u => u.Photos)
                   .AsSplitQuery()
                );

            var friends = _mapper.Map<List<UserFriend>, List<UserFriendMapDto>>(friendsMap.ToList());
            return friends;
        }

        public async Task<AppUserShortDto> GetUserDtoByEmailAsync(string email){
            var user = await _userRepo.GetByAsync<AppUser>(
                query => query.Where(u=> u.Email == email)
                              .Include(u => u.UserInfo)
                              .Include(u => u.Photos)
                              .AsSplitQuery()
                );
            var userInfoDto = _mapper.Map<AppUser,AppUserShortDto>(user);
            return userInfoDto;
        }


        public async Task<AppUserShortDto> GetUserDtoByIdAsync(int id){
            var user = await _userRepo.GetByAsync<AppUser>(
                query => query.Where(u=> u.Id == id)
                              .Include(u => u.UserInfo)
                              .Include(u => u.Photos)
                              .AsSplitQuery()
                );
            var userInfoDto = _mapper.Map<AppUser,AppUserShortDto>(user);
            return userInfoDto;
        }

         public async Task<AppUserShortDto> GetUserDtoByUserNameAsync(string username){
            var user = await GetUserByUserNameAsync(username);
            var userInfoDto = _mapper.Map<AppUser,AppUserShortDto>(user);
            return userInfoDto;
        }

        public async Task<AppUser> GetUserByUserNameAsync(string userName){
            var user = await _userRepo.GetByAsync<AppUser>(
                query => query.Where(u=> u.UserName == userName)
                              .Include(u => u.UserInfo)
                              .Include(u => u.Photos)
                              .AsSplitQuery()
                );
            return user;
        }

        public async Task<AppUserDto> GetUserDetailDtoByEmailAsync(string email){
            var user = await _userRepo.GetByAsync<AppUser>(query => 
                    query.Where(u=> u.Email == email)
                          .Include(u => u.UserInfo)
                          .Include(u => u.Address)
                          .Include(u => u.Photos)
                          .AsSplitQuery()
                          );
            var userInfoDto = _mapper.Map<AppUser,AppUserDto>(user);
            return userInfoDto;
        }


        public async Task<AppUserShortDto> GetUserDtoByPublicIdAsync(string publicId){
            var user = await _userRepo.GetByAsync<AppUser>(query => query
                          .Where(u=> u.UserPublicId == publicId)
                          .Include(u => u.UserInfo)
                          .Include(u => u.Address)
                          .Include(u => u.Photos)
                          .AsSplitQuery()
            );
            var userInfoDto = _mapper.Map<AppUser,AppUserShortDto>(user);
            return userInfoDto;
        }

        public async Task<PagedList<NotificationDto>> GetNotificationDtoAsync(int pageIndex,int pageSize,int userId){
            var notifications = await _userRepo.GetAllPagedAsync<Notification>(pageIndex,pageSize,
                query => query.Where(u => u.AppUserId == userId)
                               .Include(u => u.RequestUser)
                               .ThenInclude(u=> u.Photos)
                               .OrderBy(u => u.Id));
            
            var notificationsDto = _mapper.Map<PagedList<Notification>,PagedList<NotificationDto>>(notifications);

            return notificationsDto;
        }
        

    }
}