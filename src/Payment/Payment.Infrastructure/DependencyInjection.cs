using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }
    }
}
