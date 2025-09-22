using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Movies.Application.DTOs;
using Movies.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Movies.Application.Queries.Handlers
{
    public class GetGenresHandler : IRequestHandler<GetGenresQuery, IEnumerable<GenreDTO>>
    {
        private readonly IGenreRepository repository;
        private readonly IConfigurationProvider _mapperConfig;

        public GetGenresHandler(IGenreRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this._mapperConfig = mapper.ConfigurationProvider;
        }

        public async Task<IEnumerable<GenreDTO>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            return await repository.Query()
                .ProjectTo<GenreDTO>(_mapperConfig)
                .ToListAsync(cancellationToken);
        }
    }
}
