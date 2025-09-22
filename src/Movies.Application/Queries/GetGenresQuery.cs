using MediatR;
using Movies.Application.DTOs;

namespace Movies.Application.Queries
{
    public record GetGenresQuery() : IRequest<IEnumerable<GenreDTO>>;

}
