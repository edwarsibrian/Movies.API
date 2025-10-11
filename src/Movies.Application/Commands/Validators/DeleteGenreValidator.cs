using FluentValidation;

namespace Movies.Application.Commands.Validators
{
    public class DeleteGenreValidator : AbstractValidator<DeleteGenreCommand>
    {
        public DeleteGenreValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Genre Id must be greater than 0.");
        }
    }
}
