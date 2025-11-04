using System.Reflection;
using Financer.Application.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financer.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = configuration["MediaTRLicense"];
            // Register MediatR handlers from the Application assembly (where handlers/query types live)
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Register AutoMapper and scan the Application assembly for profiles
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ConventionsProfile>();
        });

        return services;
    }
}
