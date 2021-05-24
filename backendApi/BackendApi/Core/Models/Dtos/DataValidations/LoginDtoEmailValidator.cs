using FluentValidation;

namespace BackendApi.Core.Models.Dtos.DataValidations
{
    public class LoginDtoEmailValidator:AbstractValidator<LoginDto>
    {
          public LoginDtoEmailValidator()
       {
           RuleFor(x => x.EmailOrUserName)
            .EmailAddress()
            .NotEmpty();
       } 
    }
}