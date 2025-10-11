using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Movies.Application.DTOs;
using Movies.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Movies.Application.Common.Extensions;

namespace Movies.Application.Queries.Handlers
{
    public class GetGenresHandler : IRequestHandler<GetGenresQuery, PagedResult<GenreDTO>>
    {
        private readonly IGenreRepository repository;
        private readonly IConfigurationProvider _mapperConfig;

        public GetGenresHandler(IGenreRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this._mapperConfig = mapper.ConfigurationProvider;
        }

        public async Task<PagedResult<GenreDTO>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var queyable = repository.GetAll();

            var totalRecords= await queyable.CountAsync(cancellationToken);

            //getting items before pagination for header
            var items = await queyable
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .Paginate(request.Pagination)
                .ProjectTo<GenreDTO>(_mapperConfig)
                .ToListAsync(cancellationToken);

            //pagination
            queyable = queyable.Paginate(request.Pagination);

            return new PagedResult<GenreDTO>
            {
                TotalRecords = totalRecords,
                Items = items
            };
        }
    }
}
