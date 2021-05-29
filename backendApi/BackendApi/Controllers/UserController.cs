using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities.Identity;
using BackendApi.Core.Services.Interfaces;
using BackendApi.Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    public class UserController: BaseApiController 
    {
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserController(
            IMapper mapper,
            IPhotoService photoService,
            IUserRepository userRepo)
        {
            this._mapper = mapper;
            this._photoService = photoService;
            this._userRepo = userRepo;
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