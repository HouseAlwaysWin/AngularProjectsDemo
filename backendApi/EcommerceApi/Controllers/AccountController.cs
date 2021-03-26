using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Entities.Identity;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using System.Globalization;
using EcommerceApi.Core.Entities;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Dtos.DataValidations;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace EcommerceApi.Controllers
{

    [ApiController]
    [Route ("api/[controller]")]
    public class AccountController:BaseApiController
    {
        private readonly UserManager<ECUser> _userManager;
        private readonly SignInManager<ECUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer _localizer;
        private readonly IMapper _mapper;

        public AccountController(
            SignInManager<ECUser> signInManager,
            UserManager<ECUser> userManager,
            ITokenService tokenService,
            IStringLocalizer localizer,
            IMapper mapper
        ){
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._localizer = localizer;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpGet("getuser")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUser(){
            var email = HttpContext.User?.Claims?.FirstOrDefault(
                x=>x.Type == ClaimTypes.Email)?.Value;
            
            var user = await _userManager.FindByEmailAsync(email);

            var token = _tokenService.CreateToken(user);

            return BaseApiOk(new UserDto{
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = token});
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Login(LoginDto login){
            var lang = CultureInfo.CurrentCulture;
            var loginValidator= new LoginDtoValidator(_localizer);
            ValidationResult validateResult = await loginValidator.ValidateAsync(login);
            if(validateResult.IsValid){
                var user = await _userManager.FindByEmailAsync(login.Email);

                if(user == null) return Unauthorized(new ApiResponse(false,""));

                var result = await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);
                if(!result.Succeeded) return Unauthorized(new ApiResponse(false,""));

                return BaseApiOk(new UserDto {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = _tokenService.CreateToken(user)
                });
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

                var user = new ECUser {
                    DisplayName = register.DisplayName,
                    Email = register.Email,
                    UserName = register.DisplayName
                };
                
                var result = await _userManager.CreateAsync(user,register.Password);
                if(!result.Succeeded) {
                    return BaseApiBadRequest(result.Errors.FirstOrDefault().Description);
                }

                 return BaseApiOk( new UserDto {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = _tokenService.CreateToken(user)
                    });
            }

            return BaseApiBadRequest(validateResult.Errors.FirstOrDefault().ErrorMessage);
        }


        [HttpGet("address")]
        public async Task<ActionResult> GetUserAddress(){
            var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.Include(x=>x.Address).SingleOrDefaultAsync(x => x.Email == email);

            var addressDto = _mapper.Map<UserAddress,AddressDto>(user.Address);

            return BaseApiOk(addressDto);
        }



        [HttpPut("address")]
        public async Task<ActionResult> UpdateUserAddress(AddressDto address){
            var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.Include(x=>x.Address).SingleOrDefaultAsync(x => x.Email == email);

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