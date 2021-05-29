using System.Reflection.Metadata;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using System.Globalization;
using BackendApi.Core.Entities;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Dtos.DataValidations;
using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Data.Repositories;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using LinqToDB.Linq;
using System.Collections.Generic;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Controllers
{

    public class AccountController:BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer _localizer;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;

        public AccountController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IStringLocalizer localizer,
            IUserRepository userRepo,
            IMapper mapper
        ){
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._localizer = localizer;
            this._mapper = mapper;
            this._userRepo = userRepo;
        }

        [Authorize]
        [HttpGet("getuser")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUser(){
            try{
                var email = HttpContext.User?.Claims?.FirstOrDefault(
                    x=>x.Type == ClaimTypes.Email)?.Value;
                
                var user = await _userManager.FindByEmailAsync(email);

                var token = _tokenService.CreateToken(user);

                var result = _mapper.Map<AppUser,UserDto>(user);
                result.Token = token;

                return BaseApiOk(result);
            }catch(Exception ex){
                return BaseApiBadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getUserInfo")]
        public async Task<ActionResult> GetUserInfo(){
            try{
                var email = HttpContext.User?.Claims?.FirstOrDefault(
                    x=>x.Type == ClaimTypes.Email)?.Value;

                var user = await _userRepo.GetByAsync<AppUser>(query => {
                    return query.Where(u => u.Email == email)
                            .Include(u => u.Address)
                            .Include(u => u.UserInfo)
                            .Include(u => u.Photos);
                });

                var token = _tokenService.CreateToken(user);

                var result = _mapper.Map<AppUser,AppUserDto>(user);

                result.Token = token;

                return BaseApiOk(result);
            }catch(Exception ex){
                return BaseApiBadRequest(ex.Message);
            }
        }

        [HttpGet("getByPublicId")]
        public async Task<ActionResult> GetUserByPublicId(string publicId){
            var user = await _userRepo.GetByAsync<AppUser>(query =>  {
                    return query.Where(u => u.UserPublicId == publicId)
                        .Include(u => u.Address)
                        .Include(u => u.UserInfo)
                        .Include(u => u.Photos);
            });

            var result = _mapper.Map<AppUser,AppUserDto>(user);

            if(user  == null){
                return BaseApiOk(null);
            }

            return BaseApiOk<AppUserDto>(result);
        }

        [HttpGet("checkUserDuplicated")]
        public async Task<ActionResult> CheckUserDuplicate(string emailOrUsername){
            var user = await _userRepo.GetByAsync<AppUser>(query =>  query.Where(u => u.Email == emailOrUsername));
            if(user != null){
                return BaseApiOk(true,"Email is duplicated");
            }
            user = await _userRepo.GetByAsync<AppUser>(query =>  query.Where(u => u.UserName == emailOrUsername));
            if(user != null){
                return BaseApiOk(true,"UserName is duplicated");
            }
            return BaseApiOk(false);
        }

        [Authorize]
        [HttpPut("updatePublicId")]
        public async Task<ActionResult> UpdateUserPublicId([FromQuery]string publicId){
            var user = await _userRepo.GetByAsync<AppUser>(query =>  query.Where(u => u.UserPublicId == publicId));
            if(user != null){
                return BaseApiOk("Public Id is repeated");
            }
            else{
              var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;
            
              user = await _userManager.FindByEmailAsync(email);
            }

            await _userRepo.UpdateAsync<AppUser>(u => u.Id == user.Id,
                    new Dictionary<string,object> { 
                        { nameof(user.UserPublicId), publicId} 
                    });
            await _userRepo.CompleteAsync();

            return BaseApiOk("Update success");
        }

        

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Login(LoginDto login){
            var loginValidator= new LoginDtoValidator(_localizer);
            ValidationResult validateResult = await loginValidator.ValidateAsync(login);
            if(validateResult.IsValid){

                var emailValidator = new LoginDtoEmailValidator();
                ValidationResult emailValidateResult = await emailValidator.ValidateAsync(login);
                AppUser user = null;
                if(emailValidateResult.IsValid){
                    user = await _userManager.FindByEmailAsync(login.EmailOrUserName);
                }
                else{
                    user = await _userManager.FindByNameAsync(login.EmailOrUserName);
                }

                if(user == null) return Unauthorized(new ApiResponse(false,""));

                var result = await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);
                if(!result.Succeeded) return Unauthorized(new ApiResponse(false,""));

                 var userInfo = await _userRepo.GetByAsync<AppUser>(query =>  {
                    return query.Where(u => u.Email == user.Email)
                        .Include(u => u.Address)
                        .Include(u => u.UserInfo)
                        .Include(u => u.Photos);
                    });

                var userResult = _mapper.Map<AppUser,AppUserDto>(userInfo);
                userResult.Token = _tokenService.CreateToken(user);

                return BaseApiOk(userResult);
            }

            return BaseApiBadRequest(validateResult.Errors.FirstOrDefault().ErrorMessage);
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery]string email){
            return BaseApiOk(await _userManager.FindByEmailAsync(email)!=null);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegisterDto register){
            var registerValidator = new RegisterDtoValidator(_localizer);
            ValidationResult validateResult = await registerValidator.ValidateAsync(register);
            if(validateResult.IsValid){
                if(CheckEmailExistsAsync(register.Email).Result.Value){
                    return BaseApiBadRequest(_localizer["EmailAddressIsUsed"]);
                }

                var user = new AppUser {
                    Email = register.Email,
                    UserName = register.UserName,
                    UserInfo = new UserInfo{
                        Gender = register.Gender,
                        DateOfBirth = register.DateOfBirth
                    }
                };
                
                var result = await _userManager.CreateAsync(user,register.Password);
                if(!result.Succeeded) {
                    return BaseApiBadRequest(result.Errors.FirstOrDefault().Description);
                }

                    var userInfo = await _userRepo.GetByAsync<AppUser>(query =>  {
                    return query.Where(u => u.Email == user.Email)
                        .Include(u => u.Address)
                        .Include(u => u.UserInfo)
                        .Include(u => u.Photos);
                    });

                var userResult = _mapper.Map<AppUser,AppUserDto>(userInfo);
                userResult.Token = _tokenService.CreateToken(user);

                 return BaseApiOk(userResult);
            }

            return BaseApiBadRequest(validateResult.Errors.FirstOrDefault().ErrorMessage);
        }



        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult> GetUserAddress(){
            var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.Include(x=>x.Address).SingleOrDefaultAsyncEF(x => x.Email == email);

            var addressDto = _mapper.Map<UserAddress,AddressDto>(user.Address);

            return BaseApiOk(addressDto);
        }



        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult> UpdateUserAddress(AddressDto address){
            var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.Include(x=>x.Address)
                    .FirstOrDefaultAsyncEF(x => x.Email == email);

            user.Address = _mapper.Map<AddressDto,UserAddress>(address);

            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded){ 
                var addressDto =  _mapper.Map<UserAddress,AddressDto>(user.Address);
                return BaseApiOk(addressDto);
            }

            return BaseApiBadRequest(result.Errors.FirstOrDefault()?.Description);
        }
       
        
    }
}