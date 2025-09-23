using MediatR;
using Movies.Application.DTOs;

namespace Movies.Application.Queries
{
    public record GetGenreByIdQuery(int Id) : IRequest<GenreDTO?>;
}
