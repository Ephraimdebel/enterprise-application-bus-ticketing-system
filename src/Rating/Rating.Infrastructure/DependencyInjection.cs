using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rating.Application.Interfaces;
using Rating.Infrastructure.Persistence;

namespace Rating.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRatingInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // DbContext
        services.AddDbContext<RatingDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Repositories
        services.AddScoped<IRatingRepository, Repositories.RatingRepository>();

        return services;
    }
}
