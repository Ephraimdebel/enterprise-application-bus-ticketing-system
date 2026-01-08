using Dispute.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Dispute.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        
        services.AddDbContext<DisputeDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IDisputeRepository, DisputeRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<DisputeDbContext>());

        services.AddScoped<MessagingService>();

        services.AddHostedService<EventSubscriberService>();

        services.AddQuartz(config =>

        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            config.AddJob<ProcessOutboxMessagesJob>(jobKey);

            config.AddTrigger(trigger =>
                trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(5).RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
