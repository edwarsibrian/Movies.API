using MediatR;
using Movies.Application.DTOs;

namespace Movies.Application.Commands
{
    public record CreateGenreCommand(string genreName) : IRequest<GenreDTO>;
    
}
