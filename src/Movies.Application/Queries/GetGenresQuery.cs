using MediatR;
using Movies.Application.DTOs;

namespace Movies.Application.Queries
{
    public record GetGenresQuery(PaginationDTO Pagination) : IRequest<PagedResult<GenreDTO>>;

}
