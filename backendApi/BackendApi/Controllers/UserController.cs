using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities.Identity;
using BackendApi.Core.Services.Interfaces;
using BackendApi.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    public class UserController: BaseApiController 
    {
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(
            IMapper mapper,
            IPhotoService photoService,
            IUserRepository userRepo,
            IUserService userService)
        {
            this._mapper = mapper;
            this._photoService = photoService;
            this._userRepo = userRepo;
            this._userService = userService;
        }


        [Authorize]
        [HttpGet("get-user-detail")]
        public async Task<ActionResult> GetUserDetail() {
           var user = await _userService.GetUserDetailByEmailAsync(User.GetEmail());
           return BaseApiOk(user);
        }

        [Authorize]
        [HttpPut("update-publicId/{publicId}")]
        public async Task<ActionResult> UpdateUserPublicId(string publicId){
            var user = await _userService.GetUserByPublicIdAsync(publicId);
            if(user != null){
                return BaseApiOk("Public Id is repeated");
            }

            user = await _userService.GetUserByEmailAsync(User.GetEmail()); 

            await _userRepo.UpdateAsync<AppUser>(u => u.Id == user.Id,
                    new Dictionary<string,object> { 
                        { nameof(user.UserPublicId), publicId} 
                    });

            await _userRepo.CompleteAsync();
            user.UserPublicId = publicId;

            return BaseApiOk(user);
        }



        [HttpGet("get-by-publicId/{publicId}")]
        public async Task<ActionResult> GetUserByPublicId(string publicId){
            var user = await _userService.GetUserByPublicIdAsync(publicId);


            if(user  == null){
                return BaseApiOk(null);
            }

            return BaseApiOk<AppUserShortDto>(user);
        }



        [HttpGet("get-friends")]
        public async Task<ActionResult> GetFriends() {
            var friends =  await this._userService.GetUserFriendsByEmailAsync(User.GetEmail());
            return BaseApiOk(friends);
        }

        [HttpPost("add-friend/{friendId}")]
        public async Task<ActionResult> AddFriend(int friendId) {
            var user = await _userService.GetUserByEmailAsync(User.GetEmail());

             if(friendId == user.Id) {
                return BaseApiBadRequest("friendId can't be user");
            }

            var newFriend = new UserFriend(){
                AppUserId = user.Id,
                FriendId = friendId 
            };
            
            await _userRepo.AddAsync<UserFriend>(newFriend);

            await _userRepo.CompleteAsync();

            var friends =  await this._userService.GetUserFriendsByEmailAsync(user.Email); 

            return BaseApiOk(friends);
        }

        [HttpDelete("remove-friend/{friendId}")]
        public async Task<ActionResult> RemoveFriend(int friendId) {
            
            var user = await _userService.GetUserByEmailAsync(User.GetEmail());

            if(friendId == user.Id) {
                return BaseApiBadRequest("friendId can't be user");
            }

            await _userRepo.RemoveAsync<UserFriend>(uf => (uf.FriendId == friendId && uf.AppUserId == user.Id));
            
            await _userRepo.CompleteAsync();

            var friends =  await this._userService.GetUserFriendsByEmailAsync(user.Email); 

            return BaseApiOk(friends);
        }

        [HttpPost("add-photos")]
        public async Task<ActionResult<UserPhotoDto>> AddPhotos(IFormFile[] files)
        {
            var user = await _userRepo.GetByAsync<AppUser>(query =>
                                query.Where(u=> u.Email == User.GetEmail())
                                        .Include(u => u.Photos)
                                );

            foreach (var file in files)
            {
                
                var result = await _photoService.AddPhotoAsync(file);

                if (result.Error != null) return BadRequest(result.Error.Message);

                var photo = new UserPhoto 
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId
                };

                if (user.Photos.Count == 0)
                {
                    photo.IsMain = true;
                }

                user.Photos.Add(photo);
            }


            var uploadResult = await _userRepo.CompleteAsync();
            if(uploadResult){
                user = await _userRepo.GetByAsync<AppUser>(query =>
                    query.Where(u=> u.Email == User.GetEmail())
                            .Include(u => u.Photos));
                    var userDto = _mapper.Map<AppUser,AppUserDto>(user);
                    return BaseApiOk(userDto);
            }

            return BaseApiBadRequest("Problem addding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepo.GetByAsync<AppUser>(query =>
                query.Where(u=> u.Email == User.GetEmail())
                        .Include(u => u.Photos)
                );

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BaseApiBadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepo.CompleteAsync()) return BaseApiOk();

            return BaseApiBadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepo.GetByAsync<AppUser>(query =>
                query.Where(u=> u.Email == User.GetEmail())
                        .Include(u => u.Photos)
                );


            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return BaseApiNotFound();

            if (photo.IsMain) return BaseApiBadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BaseApiBadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepo.CompleteAsync()) return BaseApiOk();

            return BaseApiBadRequest("Failed to delete the photo");
        }

    }
}