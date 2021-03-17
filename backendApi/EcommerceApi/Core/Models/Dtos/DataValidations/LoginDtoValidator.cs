using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace EcommerceApi.Core.Models.Dtos.DataValidations
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
       public LoginDtoValidator(IStringLocalizer localizer)
       {
          RuleFor(x => x.Email)
          .NotEmpty().WithMessage(localizer["{0}IsRequired",localizer["Email"]])
          .EmailAddress().WithMessage(localizer["{0}FormatIncorrect",localizer["Email"]]);
          RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["{0}IsRequired",localizer["Password"]])
            .MinimumLength(6).WithMessage(localizer["{0}{1}MinLength",localizer["Password"],6]);
       } 
    }
}