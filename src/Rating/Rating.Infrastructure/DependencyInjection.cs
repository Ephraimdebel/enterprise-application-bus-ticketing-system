using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Rating.Application.Interfaces;
using Rating.Infrastructure.Persistence;
using Quartz;

namespace Rating.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRatingInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // DbContext
        services.AddDbContext<RatingDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            // Suppress EF Core 9+ warning about pending model changes for this demo
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        // Repositories
        services.AddScoped<IRatingRepository, Repositories.RatingRepository>();

        // Quartz for Outbox
        services.AddQuartz(q =>
        {
            var jobKey = new global::Quartz.JobKey(nameof(Outbox.ProcessOutboxMessagesJob));
            q.AddJob<Outbox.ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("Rating-Outbox-Trigger")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        // Messaging
        services.AddHostedService<Messaging.TripCompletedConsumer>();

        return services;
    }
}
