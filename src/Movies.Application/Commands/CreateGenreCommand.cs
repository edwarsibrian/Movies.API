using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Commands
{
    public record CreateGenreCommand(string name) : IRequest<Genre>;
    
}
