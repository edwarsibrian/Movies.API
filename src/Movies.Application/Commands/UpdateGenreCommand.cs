using MediatR;
using Movies.Application.DTOs;

namespace Movies.Application.Commands
{
    public record UpdateGenreCommand(int Id, string GenreName) : IRequest<GenreDTO>;

}
