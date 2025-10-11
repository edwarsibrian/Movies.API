using MediatR;
using Movies.Domain.Common.Interfaces;

namespace Movies.Application.Commands.Handlers
{
    public class DeleteGenreHandler : IRequestHandler<DeleteGenreCommand, int>
    {
        private readonly IGenreRepository _repository;

        public DeleteGenreHandler(IGenreRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
