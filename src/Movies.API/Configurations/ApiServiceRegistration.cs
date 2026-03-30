using Movies.API.Caching;
using Movies.API.Providers;
using Movies.API.Settings;
using Movies.Application.Configurations;
using Movies.Application.Interfaces;
using Movies.Infrastructure.Configurations;


namespace Movies.API.Configurations
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            // Settings
            services.Configure<CorsSettings>(configuration.GetSection("Cors"));
                        
            var corsSettings = configuration
                .GetSection("Cors")
                .Get<CorsSettings>() ?? throw new InvalidOperationException("Cors settings are not configured properly.");

            //Add CORS policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(corsSettings.AllowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("TotalRecords");
                });
            });

            // Cache
            services.AddOutputCache(opt =>
            {
                opt.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
            });
            services.AddScoped<ICacheService, OutputCacheService>();

            // Providers
            services.AddSingleton<IWebRootPathProvider, WebRootPathProvider>();

            // Application
            services.AddApplication();

            // Infrastructure
            services.AddInfrastructure(configuration);                      

            return services;
        }

    }
}
