using BusProvider.Domain.Interfaces;
using BusProvider.Infrastructure.Persistence;
using BusProvider.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusProvider.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BusProviderDb")
            ?? "Host=localhost;Port=5432;Database=busprovider;Username=busprovider;Password=busprovider";

        services.AddDbContext<BusProviderDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IBusProviderRepository, BusProviderRepository>();
        services.AddScoped<IBusRepository, BusRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
