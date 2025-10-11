using MediatR;

namespace Movies.Application.Commands
{
    public record DeleteGenreCommand(int Id) : IRequest<int>;

}
