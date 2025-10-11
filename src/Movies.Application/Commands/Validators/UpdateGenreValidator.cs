using FluentValidation;
using Movies.Application.Common.Validators;

namespace Movies.Application.Commands.Validators
{
    public class UpdateGenreValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Genre Id must be greater than 0.");
            RuleFor(x => x.GenreName)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Must(CommonValidators.StartsWithUppercase).WithMessage("{PropertyName} must start with an uppercase letter.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}
