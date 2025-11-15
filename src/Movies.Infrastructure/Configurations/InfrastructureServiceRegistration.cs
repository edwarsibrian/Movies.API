using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Interfaces;
using Movies.Infrastructure.Services;
using Movies.Infrastructure.Services.FileStorages;
using Movies.Infrastructure.Settings;
using Movies.Repository.Configurations;

namespace Movies.Infrastructure.Configurations
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Configuration typed
            services.Configure<FileStorageSettings>(configuration.GetSection("FileStorageSettings"));
            services.Configure<SyncSettings>(configuration.GetSection("SyncSettings"));

            services.AddTransient<AzureFileStorageService>();
            services.AddTransient<LocalFileStorageService>();

            //Register decorator Fallback with IFileStorageService
            services.AddTransient<IFileStorageService, FallbackFileStorageService>();

            //Register Hosted Services of sinchronization
            services.AddHostedService<SyncHostedService>();

            // Repositories
            services.AddRepositories(configuration);

            return services;
        }
    }
}
