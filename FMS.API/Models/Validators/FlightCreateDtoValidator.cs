using FluentValidation;
using FMS.API.Entities;

namespace FMS.API.Models.Validators;

public class FlightCreateDtoValidator : AbstractValidator<FlightCreateDto>
{
    public FlightCreateDtoValidator(FMSDbContext dbContext)
    {
        RuleFor(x => x.NumerLotu)
            .NotEmpty()
            .Custom((value, context) =>
            {
                var NumerLotuInUse = dbContext.Flights.Any(u => u.NumerLotu == value);

                if (NumerLotuInUse)
                {
                    context.AddFailure("NumerLotu", "NumerLotu already exists.");
                }
            });

        RuleFor(x => x.MiejsceWylotu)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.MiejscePrzylotu)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.TypSamolotu)
            .IsInEnum();
    }
}
