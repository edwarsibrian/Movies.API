using Microsoft.EntityFrameworkCore;
using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;
using Movies.Infrastructure.Context;

namespace Movies.Repository.Implementations
{
    public class GenreRepository : IGenreRepository
    {
        private readonly APIMovieDbContext dbContext;

        public GenreRepository(APIMovieDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        public async Task CreateAsync(Genre genre, CancellationToken cancellationToken)
        {
            await dbContext.Genres.AddAsync(genre, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<Genre> Query()
        {
            return dbContext.Genres.AsNoTracking();
        }
    }
}
