using AutoMapper;
using MediatR;
using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;

namespace Movies.Application.Commands.Handlers
{
    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, Genre>
    {
        private readonly IGenreRepository repository;
        private readonly IMapper mapper;

        public CreateGenreHandler(IGenreRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Genre> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = mapper.Map<Genre>(request.name);
            await repository.CreateAsync(genre, cancellationToken);
            return genre;
        }
    }
}
