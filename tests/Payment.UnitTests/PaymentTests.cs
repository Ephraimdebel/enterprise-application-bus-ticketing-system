using Payment.Domain.Entities;
using Payment.Domain.ValueObjects;
using Payment.Domain.Events;
using Xunit;

namespace Payment.UnitTests;

public class PaymentTests
{
    [Fact]
    public void Constructor_ShouldSetStatusToPending()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var amount = new Money(250, "ETB");
        var method = PaymentMethod.Card;

        // Act
        var payment = new PaymentEntity(bookingId, amount, method);

        // Assert
        Assert.Equal("Pending", payment.Status);
        Assert.Equal(bookingId, payment.BookingId);
    }

    [Fact]
    public void MarkAsSuccess_ShouldSetStatusToConfirmed_AndRaiseDomainEvent()
    {
        // Arrange
        var payment = CreatePendingPayment();

        // Act
        payment.MarkAsSuccess();

        // Assert
        Assert.Equal("Confirmed", payment.Status);
        Assert.Contains(payment.GetDomainEvents(), e => e is PaymentCompleted);
    }

    [Fact]
    public void MarkAsFailed_ShouldSetStatusToFailed()
    {
        // Arrange
        var payment = CreatePendingPayment();

        // Act
        payment.MarkAsFailed();

        // Assert
        Assert.Equal("Failed", payment.Status);
    }

    private PaymentEntity CreatePendingPayment()
    {
        return new PaymentEntity(
            Guid.NewGuid(),
            new Money(250, "ETB"),
            PaymentMethod.Cash);
    }
}
