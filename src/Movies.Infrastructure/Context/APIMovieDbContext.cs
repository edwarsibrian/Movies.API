using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;

namespace Movies.Infrastructure.Context
{
    public class APIMovieDbContext : DbContext
    {
        public APIMovieDbContext(DbContextOptions options) : base(options)
        {
        }

        protected APIMovieDbContext()
        {
        }

        public DbSet<Genre> Genres { get; set; }
    }
}
