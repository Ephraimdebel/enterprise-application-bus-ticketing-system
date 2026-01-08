using Microsoft.EntityFrameworkCore;
using TripAggregate = Trip.Domain.Aggregates.Trip;
using Trip.Domain.Entities;

namespace Trip.Infrastructure.Persistence;

public sealed class TripDbContext : DbContext
{
    public TripDbContext(DbContextOptions<TripDbContext> options)
        : base(options) { }

    public DbSet<TripAggregate> Trips => Set<TripAggregate>();
    public DbSet<Seat> Seats => Set<Seat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TripDbContext).Assembly);
    }
}


