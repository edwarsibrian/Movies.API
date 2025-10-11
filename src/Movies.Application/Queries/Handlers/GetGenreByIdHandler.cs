using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.Application.DTOs;
using Movies.Domain.Common.Interfaces;

namespace Movies.Application.Queries.Handlers
{
    public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdQuery, GenreDTO?>
    {
        private readonly IGenreRepository _repository;
        private readonly IConfigurationProvider _mapperConfig;

        public GetGenreByIdHandler(IGenreRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapperConfig = mapper.ConfigurationProvider;
        }

        public async Task<GenreDTO?> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAll()
                .Where(g => g.Id == request.Id)
                .ProjectTo<GenreDTO>(_mapperConfig)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
