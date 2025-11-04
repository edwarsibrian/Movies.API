using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Repository.Configurations;

namespace Movies.Repository.Context
{
    public class APIMovieDbContext : DbContext
    {
        public APIMovieDbContext(DbContextOptions<APIMovieDbContext> options) : base(options)
        {
        }

        protected APIMovieDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ActorConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
    }
}
