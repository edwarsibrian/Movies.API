using Microsoft.EntityFrameworkCore;
using Movies.Domain.Common.Interfaces;
using Movies.Domain.Entities;
using Movies.Repository.Context;

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

        public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return await dbContext.Genres
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public IQueryable<Genre> GetAll()
        {
            return dbContext.Genres.AsNoTracking();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns false if the genre does not exist</returns>
        public async Task<bool> UpdateAsync(Genre genre, CancellationToken cancellationToken)
        {
            bool exists = await dbContext.Genres.AnyAsync(g => g.Id == genre.Id, cancellationToken);
            if (!exists)
            {
                return false;
            }
            dbContext.Genres.Update(genre);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
