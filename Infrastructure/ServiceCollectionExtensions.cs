using Financer.Domain.Repositories;
using Financer.Infrastructure.Authentication;
using Financer.Infrastructure.Configuration;
using Financer.Infrastructure.Data;
using Financer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financer.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Add PostgreSQL DbContext
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        // Configure JWT settings
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        // Register services
        services.AddScoped<ITokenService, TokenService>();

        // Register repositories
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}
