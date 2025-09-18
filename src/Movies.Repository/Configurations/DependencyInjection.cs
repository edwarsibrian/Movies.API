using Microsoft.Extensions.DependencyInjection;
using Movies.Domain.Common.Interfaces;
using Movies.Repository.Implementations;

namespace Movies.Repository.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IGenreRepository, GenreRepository>();
            
            return services;
        }
    }
}
