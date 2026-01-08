using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Passenger.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPassengerApplication(this IServiceCollection services)
    {
        // MediatR v11 registration
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        return services;
    }
}
