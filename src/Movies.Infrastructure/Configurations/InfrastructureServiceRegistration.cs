using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Movies.Application.Interfaces;
using Movies.Infrastructure.Services;
using Movies.Infrastructure.Services.FileStorages;
using Movies.Infrastructure.Services.Resilience.Policies;
using Movies.Infrastructure.Settings;
using Movies.Repository.Configurations;
using Polly;

namespace Movies.Infrastructure.Configurations
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // ----------------------------
            // Configuration (Typed Settings)
            // ----------------------------
            services.Configure<FileStorageSettings>(configuration.GetSection("FileStorageSettings"));
            services.Configure<SyncSettings>(configuration.GetSection("SyncSettings"));

            // ----------------------------
            // External Services (Azure, etc.)
            // ----------------------------
            services.AddSingleton<BlobServiceClient>(sp =>
            {
                var settings = sp
                .GetRequiredService<IOptions<FileStorageSettings>>().Value;
                
                return new BlobServiceClient(settings.ConnectionString);
            });

            // ----------------------------
            // Resilience Policies (Polly)
            // ----------------------------
            services.AddSingleton<IAsyncPolicy>(RetryPolicies.DefaultRetry);

            // ----------------------------
            // File Storage Services
            // ----------------------------
            services.AddTransient<AzureFileStorageService>();
            services.AddTransient<LocalFileStorageService>();

            // Decorator: Fallback (Azure -> Local)
            services.AddTransient<IFileStorageService, FallbackFileStorageService>();

            // ----------------------------
            // Background Services
            // ----------------------------
            services.AddHostedService<SyncHostedService>();

            // ----------------------------
            // Repositories
            // ----------------------------
            services.AddRepositories(configuration);

            return services;
        }
    }
}
