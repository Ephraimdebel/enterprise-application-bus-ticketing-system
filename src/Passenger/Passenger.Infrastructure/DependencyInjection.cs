using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passenger.Application.Abstractions;
using Passenger.Domain.Repositories;
using Passenger.Infrastructure.Outbox;
using Passenger.Infrastructure.Persistence.DbContext;
using Passenger.Infrastructure.Repositories;
using Passenger.Infrastructure.Time;
using Quartz;

namespace Passenger.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPassengerInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PassengerDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("PassengerDb");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'PassengerDb' is not configured.");

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IPassengerRepository, PassengerRepository>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        services.Configure<OutboxProcessingOptions>(opts =>
            configuration.GetSection("OutboxProcessing").Bind(opts));

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("Passenger.OutboxProcessor");
            q.AddJob<ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("Passenger.OutboxProcessor.Trigger")
                .StartNow()
                .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(5)).RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
