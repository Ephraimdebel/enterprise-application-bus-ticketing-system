using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payment.Domain.Repositories;
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

            return services;
        }
    }
}
