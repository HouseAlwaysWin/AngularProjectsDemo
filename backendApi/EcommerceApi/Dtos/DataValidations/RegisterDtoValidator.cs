using System.Data;
using FluentValidation;

namespace EcommerceApi.Dtos.DataValidations
{
    public class RegisterDtoValidator:AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
           RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Can't Not Empty");
           RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Can't Not Empty")
                .EmailAddress().WithMessage("Incorrect Email Address");
           RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Can't Not Empty")
                .MinimumLength(6).WithMessage("Minimum Length is 6");
        }
    }
}