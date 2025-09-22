using FluentValidation;

namespace Movies.Application.Queries.Validators
{
    public class GetGenreByIdValidator : AbstractValidator<GetGenreByIdQuery>
    {
        public GetGenreByIdValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Genre Id must be greater than 0.");
        }
    }
}
