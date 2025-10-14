using MediatR;

namespace Movies.Application.Commands
{
    public record CreateActorCommand(
        string Name,
        DateTime DateOfBirth,
        string? Picture
        ) : IRequest<int>;
}
