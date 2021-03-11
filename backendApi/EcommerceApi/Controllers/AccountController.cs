using System.Threading;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Entities.Identity;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using EcommerceApi.Core.ErrorHandlers;
using Microsoft.AspNetCore.Http;
using EcommerceApi.Dtos.DataValidations;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System;
using System.Globalization;

namespace EcommerceApi.Controllers
{

    [ApiController]
    [Route ("api/[controller]")]
    public class AccountController:ControllerBase
    {
        private readonly UserManager<ECUser> _userManager;
        private readonly SignInManager<ECUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer _localizer;

        public AccountController(
            SignInManager<ECUser> signInManager,
            UserManager<ECUser> userManager,
            ITokenService tokenService,
            IStringLocalizer<AccountController> localizer
        ){
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._localizer = localizer;
        }

        [Authorize]
        [HttpGet("GetUserState")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserState(){
            var email = HttpContext.User?.Claims?.FirstOrDefault(
                x=>x.Type == ClaimTypes.Email)?.Value;
            
            var user = await _userManager.FindByEmailAsync(email);

            var token = _tokenService.CreateToken(user);

            return new ApiResponse<UserDto>{
                Data = new UserDto{
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = token
                }
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Login(LoginDto login){
            var loginValidator= new LoginDtoValidator();
            ValidationResult validateResult = await loginValidator.ValidateAsync(login);
            if(validateResult.IsValid){
                var user = await _userManager.FindByEmailAsync(login.Email);

                if(user == null) return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));

                var result = await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);
                if(!result.Succeeded) return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));

                return new ApiResponse<UserDto> {
                    Data = new UserDto {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = _tokenService.CreateToken(user)
                    },
                };
            }

            return new ApiResponse<UserDto> {
                    StatusCode= StatusCodes.Status400BadRequest,
                    Message=validateResult.Errors.FirstOrDefault().ErrorMessage
                     };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery]string email){
            return await _userManager.FindByEmailAsync(email)!=null;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegisterDto register){
            var registerValidator = new RegisterDtoValidator();
            ValidationResult validateResult = await registerValidator.ValidateAsync(register);
            if(validateResult.IsValid){
                if(CheckEmailExistsAsync(register.Email).Result.Value){
                    return BadRequest(
                        new ApiResponse(StatusCodes.Status400BadRequest,
                        _localizer["EmailAddressIsUsed"]));
                }

                var user = new ECUser {
                    DisplayName = register.DisplayName,
                    Email = register.Email,
                    UserName = register.DisplayName
                };
                
                var result = await _userManager.CreateAsync(user,register.Password);
                if(!result.Succeeded) {
                    return BadRequest(
                    new ApiResponse(
                        StatusCodes.Status400BadRequest,
                        result.Errors.FirstOrDefault().Description));
                }

                 return new ApiResponse<UserDto> {
                    Data = new UserDto {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = _tokenService.CreateToken(user)
                    },
                };
            }

            return new ApiResponse<UserDto> {
                    StatusCode= StatusCodes.Status400BadRequest,
                    Message=validateResult.Errors.FirstOrDefault().ErrorMessage 
                };
        }

       
        
    }
}