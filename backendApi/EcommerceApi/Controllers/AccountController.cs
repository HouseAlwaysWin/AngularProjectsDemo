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
            IStringLocalizer localizer
        ){
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._localizer = localizer;
        }

        [Authorize]
        [HttpGet("getuser")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUser(){
            var email = HttpContext.User?.Claims?.FirstOrDefault(
                x=>x.Type == ClaimTypes.Email)?.Value;
            
            var user = await _userManager.FindByEmailAsync(email);

            var token = _tokenService.CreateToken(user);

            return new ApiResponse<UserDto>(
                true,
                new UserDto{
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

                if(user == null) return Unauthorized(new ApiResponse(""));

                var result = await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);
                if(!result.Succeeded) return Unauthorized(new ApiResponse(""));

                return Ok(new ApiResponse<UserDto> (
                     true,
                     new UserDto {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = _tokenService.CreateToken(user)
                }));
            }

            return BadRequest(new ApiResponse (
                    validateResult.Errors.FirstOrDefault().ErrorMessage
            ));
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery]string email){
            return Ok(await _userManager.FindByEmailAsync(email)!=null);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegisterDto register){
            var registerValidator = new RegisterDtoValidator(_localizer);
            ValidationResult validateResult = await registerValidator.ValidateAsync(register);
            if(validateResult.IsValid){
                if(CheckEmailExistsAsync(register.Email).Result.Value){
                    return BadRequest(
                        new ApiResponse(_localizer["EmailAddressIsUsed"]));
                }

                var user = new ECUser {
                    DisplayName = register.DisplayName,
                    Email = register.Email,
                    UserName = register.DisplayName
                };
                
                var result = await _userManager.CreateAsync(user,register.Password);
                if(!result.Succeeded) {
                    return BadRequest(
                    new ApiResponse(result.Errors.FirstOrDefault().Description));
                }

                 return Ok(new ApiResponse<UserDto> (
                     true,
                     new UserDto {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = _tokenService.CreateToken(user)
                    }
                 ));
            }

            return BadRequest(new ApiResponse( 
                    validateResult.Errors.FirstOrDefault().ErrorMessage 
                ));
        }

       
        
    }
}