using FluentValidation;
using FMS.API.Entities;

namespace FMS.API.Models.Validators;

public class UserSignupDtoValidator : AbstractValidator<UserSignupDto>
{
    public UserSignupDtoValidator(FMSDbContext dbContext)
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty();
    }
}
