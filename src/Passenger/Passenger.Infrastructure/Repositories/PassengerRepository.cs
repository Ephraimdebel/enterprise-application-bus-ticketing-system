using Microsoft.EntityFrameworkCore;
using Passenger.Domain.Aggregates;
using Passenger.Domain.Entities;
using Passenger.Domain.Repositories;
using Passenger.Domain.ValueObjects;
using Passenger.Infrastructure.Persistence.DbContext;
using PassengerAggregateRoot = Passenger.Domain.Aggregates.Passenger;

namespace Passenger.Infrastructure.Repositories;

public sealed class PassengerRepository : IPassengerRepository
{
    private readonly PassengerDbContext _db;

    public PassengerRepository(PassengerDbContext db) => _db = db;

    public Task<Passenger.Domain.Aggregates.Passenger?> GetByIdAsync(PassengerId id, CancellationToken cancellationToken = default)
        => _db.Passengers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<bool> ExistsWithEmailAsync(Email email, PassengerId? excludingPassengerId = null, CancellationToken cancellationToken = default)
    {
        var query = _db.Passengers.AsQueryable().Where(p => p.Email == email);

        if (excludingPassengerId is { } exId)
            query = query.Where(p => p.Id != exId);

        return query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(Passenger.Domain.Aggregates.Passenger passenger, CancellationToken cancellationToken = default)
        => await _db.Passengers.AddAsync(passenger, cancellationToken);

    public void Update(Passenger.Domain.Aggregates.Passenger passenger)
        => _db.Passengers.Update(passenger);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);
}
