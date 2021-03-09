using FluentValidation;

namespace EcommerceApi.Dtos.DataValidations
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
       public LoginDtoValidator()
       {
          RuleFor(x => x.Email).EmailAddress().WithMessage("");
          RuleFor(x => x.Password).MinimumLength(6).WithMessage("");
       } 
    }
}