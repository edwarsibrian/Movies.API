using FluentValidation;

namespace Movies.Application.Commands.Validation
{
    public class CreateActorValidator : AbstractValidator<CreateActorCommand>
    {
        public CreateActorValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("{PropertyName} must be a date in the past.");
        }
    }
}
