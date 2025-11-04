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
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        return services;
    }
}
