using FluentValidation;
using Movies.Application.Common.Validation;

namespace Movies.Application.Commands.Validation
{
    public class CreateGenreValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreValidator()
        {
            RuleFor(x => x.genreName)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Must(CommonValidators.StartsWithUppercase).WithMessage("{PropertyName} must start with an uppercase letter.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }        
    }
}
