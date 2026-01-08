using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Passenger.Domain.Aggregates;
using Passenger.Domain.Entities;
using Passenger.Domain.ValueObjects;

namespace Passenger.Infrastructure.Persistence.EF_Configurations;

public sealed class PassengerConfiguration : IEntityTypeConfiguration<Passenger.Domain.Aggregates.Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger.Domain.Aggregates.Passenger> builder)
    {
        builder.ToTable("passengers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => PassengerId.FromGuid(value))
            .ValueGeneratedNever();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        // Name as owned type (two columns)
        builder.OwnsOne(x => x.Name, nb =>
        {
            nb.Property(n => n.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            nb.Property(n => n.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
        });

        // Email as value-converted single column with unique index
        var emailConverter = new ValueConverter<Email, string>(
            v => v.Value,
            v => Email.Create(v));

        builder.Property(x => x.Email)
            .HasConversion(emailConverter)
            .HasColumnName("email")
            .HasMaxLength(320)
            .IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();

        // Phone as owned type (two columns)
        builder.OwnsOne(x => x.PhoneNumber, pb =>
        {
            pb.Property(p => p.CountryCode).HasColumnName("phone_country_code").HasMaxLength(4).IsRequired();
            pb.Property(p => p.Number).HasColumnName("phone_number").HasMaxLength(14).IsRequired();
        });

        // Status as value-converted int column
        var statusConverter = new ValueConverter<PassengerStatus, int>(
            v => (int)v.Code,
            v => PassengerStatus.From((PassengerStatusCode)v));

        builder.Property(x => x.Status)
            .HasConversion(statusConverter)
            .HasColumnName("status")
            .IsRequired();

        // Ignore domain events
        builder.Ignore(x => x.DomainEvents);
    }
}
