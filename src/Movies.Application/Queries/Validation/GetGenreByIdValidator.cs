using FluentValidation;

namespace Movies.Application.Queries.Validation
{
    public class GetGenreByIdValidator : AbstractValidator<GetGenreByIdQuery>
    {
        public GetGenreByIdValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}
