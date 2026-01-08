using Dispute.Domain;
using Xunit;

namespace Dispute.UnitTests;

public class DisputeTests
{
    [Fact]
    public void Open_Should_InitializeDisputeCorrectly()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var passengerId = Guid.NewGuid();
        var reason = new DisputeReason("LateBus", "The bus was late by 2 hours.");
        var initialMessage = "I want a refund.";

        // Act
        var dispute = Domain.Dispute.Open(bookingId, passengerId, reason, initialMessage);

        // Assert
        Assert.NotEqual(Guid.Empty, dispute.Id);
        Assert.Equal(bookingId, dispute.BookingId);
        Assert.Equal(passengerId, dispute.PassengerId);
        Assert.Equal(reason, dispute.Reason);
        Assert.Equal(DisputeStatus.Opened, dispute.Status);
        Assert.Single(dispute.Messages);
        Assert.Equal("Passenger", dispute.Messages.First().SenderRole);
        Assert.Equal(initialMessage, dispute.Messages.First().MessageText);
        Assert.Single(dispute.GetDomainEvents());
        Assert.IsType<DisputeOpenedDomainEvent>(dispute.GetDomainEvents().First());
    }

    [Fact]
    public void AddMessage_Should_AddMessage_When_DisputeIsOpened()
    {
        // Arrange
        var dispute = CreateDispute();

        // Act
        dispute.AddMessage("Support", "We are looking into it.");

        // Assert
        Assert.Equal(2, dispute.Messages.Count);
        Assert.Equal("Support", dispute.Messages.Last().SenderRole);
    }

    [Fact]
    public void AddMessage_Should_Throw_When_DisputeIsClosed()
    {
        // Arrange
        var dispute = CreateDispute();
        dispute.ChangeStatus(DisputeStatus.Resolved);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => dispute.AddMessage("Passenger", "Thanks."));
    }

    [Fact]
    public void ChangeStatus_Should_UpdateStatus()
    {
        // Arrange
        var dispute = CreateDispute();

        // Act
        dispute.ChangeStatus(DisputeStatus.InReview);

        // Assert
        Assert.Equal(DisputeStatus.InReview, dispute.Status);
    }

    [Fact]
    public void ChangeStatus_Should_SetResolvedAt_When_Resolved()
    {
        // Arrange
        var dispute = CreateDispute();

        // Act
        dispute.ChangeStatus(DisputeStatus.Resolved);

        // Assert
        Assert.Equal(DisputeStatus.Resolved, dispute.Status);
        Assert.NotNull(dispute.ResolvedAt);
    }

    private static Domain.Dispute CreateDispute()
    {
        return Domain.Dispute.Open(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DisputeReason("Test", "Test"),
            "Initial message");
    }
}
