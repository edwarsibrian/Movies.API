using MediatR;
using Movies.Application.DTOs;

namespace Movies.Application.Queries.Handlers
{
    public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdQuery, GenreDTO?>
    {
        public GetGenreByIdHandler()
        {
        }

        public Task<GenreDTO?> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
