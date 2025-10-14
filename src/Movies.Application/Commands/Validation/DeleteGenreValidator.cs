using FluentValidation;

namespace Movies.Application.Commands.Validation
{
    public class DeleteGenreValidator : AbstractValidator<DeleteGenreCommand>
    {
        public DeleteGenreValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}
