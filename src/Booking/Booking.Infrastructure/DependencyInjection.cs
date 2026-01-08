using Microsoft.EntityFrameworkCore;
using Booking.Application;
using global::Booking.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Booking.Application.Interfaces;

namespace Booking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ?? 
                               throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<BookingDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BookingDbContext>());
        services.AddScoped<MessagingService>();

        // --- Outbox Service ---
        services.AddScoped<IOutboxService, OutboxService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(10).RepeatForever()));
        });

        services.AddQuartzHostedService();

        return services;
    }
}
