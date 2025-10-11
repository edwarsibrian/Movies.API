using AutoMapper;
using MediatR;
using Movies.Application.DTOs;
using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;

namespace Movies.Application.Commands.Handlers
{
    public class UpdateGenreHandler : IRequestHandler<UpdateGenreCommand, GenreDTO?>
    {
        private readonly IGenreRepository _repository;
        private readonly IMapper _mapper;

        public UpdateGenreHandler(IGenreRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GenreDTO?> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = _mapper.Map<Genre>(request);
            bool updated = await _repository.UpdateAsync(genre, cancellationToken);
            if (updated)
            {
                return _mapper.Map<GenreDTO>(genre);
            }
            else
            {
                return null;
            }
        }
    }
}
