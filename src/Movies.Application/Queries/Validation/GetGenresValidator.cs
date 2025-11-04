using FluentValidation;
using Movies.Application.Common.Validation;

namespace Movies.Application.Queries.Validation
{
    public class GetGenresValidator : AbstractValidator<GetGenresQuery>
    {
        public GetGenresValidator()
        {
            RuleFor(x=>x.Pagination)
                .SetValidator(new PaginationDTOValidator());
        }
    }
}
