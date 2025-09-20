using FluentValidation;

namespace Movies.Application.Commands.Validators
{
    public class CreateGenreValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreValidator()
        {
            RuleFor(x => x.genreName)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Must(StartsWithUppercase).WithMessage("{PropertyName} must start with an uppercase letter.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }

        private bool StartsWithUppercase(string genreName)
        {
            if (string.IsNullOrEmpty(genreName))
                return false;
            return char.IsUpper(genreName[0]);
        }
    }
}
