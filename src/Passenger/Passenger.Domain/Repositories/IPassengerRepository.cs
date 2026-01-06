using PassengerAggregate = Passenger.Domain.Aggregates.Passenger;
using PassengerId = Passenger.Domain.Entities.PassengerId;
using Passenger.Domain.ValueObjects;

namespace Passenger.Domain.Repositories
{
    public interface IPassengerRepository
    {
        Task<PassengerAggregate?> GetByIdAsync(
            PassengerId id,
            CancellationToken cancellationToken = default
        );

        Task<bool> ExistsWithEmailAsync(
            Email email,
            PassengerId? excludingPassengerId = null,
            CancellationToken cancellationToken = default
        );

        Task AddAsync(
            PassengerAggregate passenger,
            CancellationToken cancellationToken = default
        );

        void Update(PassengerAggregate passenger);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
