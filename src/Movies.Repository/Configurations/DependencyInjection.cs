using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Domain.Common.Interfaces;
using Movies.Repository.Context;
using Movies.Repository.Implementations;

namespace Movies.Repository.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<APIMovieDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

            // Repositories
            services.AddScoped<IGenreRepository, GenreRepository>();
            
            return services;
        }
    }
}
