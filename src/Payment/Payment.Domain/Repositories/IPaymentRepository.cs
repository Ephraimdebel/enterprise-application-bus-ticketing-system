using Payment.Domain.Entities;

namespace Payment.Domain.Repositories;

public interface IPaymentRepository
{
    Task<PaymentEntity> GetByIdAsync(Guid id);
    Task AddAsync(PaymentEntity payment);
    Task SaveChangesAsync();
}
