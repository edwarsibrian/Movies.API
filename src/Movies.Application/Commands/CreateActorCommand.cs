using MediatR;
using Movies.Application.DTOs;

namespace Movies.Application.Commands
{
    public record CreateActorCommand(
        string Name,
        DateTime DateOfBirth,
        string? Picture
        ) : IRequest<ActorDTO>;
}
