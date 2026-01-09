using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Payment.Application.Interfaces;
using Payment.Domain.Repositories;
using Payment.Infrastructure.Messaging;
using Payment.Infrastructure.Persistence;
using Payment.Infrastructure.Repositories;

namespace Payment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPaymentInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddSingleton<IEventPublisher, RabbitMQPublisher>();
            // services.AddSingleton<BookingConfirmedForPaymentConsumer>();
            services.AddHostedService<BookingConfirmedConsumer>();

            // Quartz for Outbox
            services.AddQuartz(q =>
            {
                var jobKey = new global::Quartz.JobKey(nameof(Outbox.ProcessOutboxMessagesJob));
                q.AddJob<Outbox.ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("Payment-Outbox-Trigger")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
