using FluentValidation;
using Movies.Application.DTOs;

namespace Movies.Application.Common.Validation
{
    public class PaginationDTOValidator : AbstractValidator<PaginationDTO>
    {
        public PaginationDTOValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            RuleFor(x => x.RecordsByPage)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}
