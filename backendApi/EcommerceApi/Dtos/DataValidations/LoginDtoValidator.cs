using FluentValidation;

namespace EcommerceApi.Dtos.DataValidations
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
       public LoginDtoValidator()
       {
          RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Can't Not Empty")
          .EmailAddress().WithMessage("Incorrect Email Address");
          RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Can't Not Empty")
            .MinimumLength(6).WithMessage("Minimum Length is 6");
       } 
    }
}