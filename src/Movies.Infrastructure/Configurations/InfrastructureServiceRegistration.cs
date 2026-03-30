using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Movies.Application.Interfaces;
using Movies.Infrastructure.HealthCheck;
using Movies.Infrastructure.Messaging;
using Movies.Infrastructure.Messaging.Consumers;
using Movies.Infrastructure.Services;
using Movies.Infrastructure.Services.FileStorages;
using Movies.Infrastructure.Services.Resilience.Policies;
using Movies.Infrastructure.Settings;
using Movies.Repository.Configurations;
using Polly;
using RabbitMQ.Client;

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
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));

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
            // Messaging Services (RabbitMQ, etc.)
            // ----------------------------
            services.AddSingleton<ConnectionFactory>(sp =>
            {                 
                var settings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                return new ConnectionFactory
                {
                    HostName = settings.HostName,
                    UserName = settings.UserName,
                    Password = settings.Password,
                    Port = settings.Port,
                    AutomaticRecoveryEnabled = true,
                    ConsumerDispatchConcurrency = 1
                };
            });
            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

            // ----------------------------
            // Health Checks (Infra)
            // ----------------------------
            var fileSettings = configuration.GetSection("FileStorageSettings").Get<FileStorageSettings>();

            services.AddHealthChecks()
                // Azure Blob
                .AddAzureBlobStorage(
                    connectionString: fileSettings.ConnectionString,
                    containerName: fileSettings.HealthCheckContainer,
                    name: "Azure Blob Storage",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "storage", "azure" }
                )
                // RabbitMQ 
                .AddCheck<RabbitMqHealthCheck>(
                    "RabbitMQ",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "messaging", "rabbitmq" }
                );

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
            services.AddHostedService<ActorImageConsumer>();

            // ----------------------------
            // Repositories
            // ----------------------------
            services.AddRepositories(configuration);

            return services;
        }
    }
}
