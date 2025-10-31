using Microsoft.Extensions.Options;
using Movies.API.Settings;
using Movies.Application.Configurations;
using Movies.Infrastructure.Configurations;


namespace Movies.API.Configurations
{
    public static class IoCConfigurator
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Settings
            var movieApiSettings = services.Configure<MovieApiSettings>(configuration.GetSection(nameof(MovieApiSettings)))
                .BuildServiceProvider().GetRequiredService<IOptions<MovieApiSettings>>().Value;

            // Add Cors
            var corsPolicy = movieApiSettings.AllowedHosts.Split(',');
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(corsPolicy)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("TotalRecords");
                });
            });

            // Add cache service
            services.AddOutputCache(opt =>
            {
                opt.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
            });

            // Application
            services.AddApplication();

            // Infrastructure
            services.AddInfrastructure(configuration);
                        
            // Services
            return services;
        }

    }
}
