using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Queries
{
    public record GetGenreByIdQuery(int Id) : IRequest<Genre?>;
}
