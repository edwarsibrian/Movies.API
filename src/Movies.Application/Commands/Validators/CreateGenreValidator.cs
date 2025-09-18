using FluentValidation;

namespace Movies.Application.Commands.Validators
{
    public class CreateGenreValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Genre name must not be empty.")
                .MaximumLength(50).WithMessage("Genre name must not exceed 50 characters.");
        }
    }
}
