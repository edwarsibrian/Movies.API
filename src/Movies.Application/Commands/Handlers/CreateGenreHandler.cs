using AutoMapper;
using MediatR;
using Movies.Application.DTOs;
using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;

namespace Movies.Application.Commands.Handlers
{
    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, GenreDTO>
    {
        private readonly IGenreRepository repository;
        private readonly IMapper mapper;

        public CreateGenreHandler(IGenreRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<GenreDTO> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = mapper.Map<Genre>(request.genreName);
            await repository.CreateAsync(genre, cancellationToken);
            var genreDto = mapper.Map<GenreDTO>(genre);
            return genreDto;
        }
    }
}
