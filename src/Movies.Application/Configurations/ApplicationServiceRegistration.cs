using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Common.Behaviors;
using System.Reflection;

namespace Movies.Application.Configurations
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Add AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());

            // Fluent Validation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
