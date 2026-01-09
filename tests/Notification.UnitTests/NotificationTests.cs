using Notification.Domain.Entities;
using Xunit;

namespace Notification.UnitTests;

public class NotificationTests
{
    [Fact]
    public void NotificationEntity_ShouldInitializeWithDefaultValues()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = "Test Message";

        // Act
        var notification = new NotificationEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message
        };

        // Assert
        Assert.False(notification.IsRead);
        Assert.Equal(userId, notification.UserId);
        Assert.Equal(message, notification.Message);
        Assert.True(notification.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void IsRead_ShouldBeSettable()
    {
        // Arrange
        var notification = new NotificationEntity { Message = "Test" };

        // Act
        notification.IsRead = true;

        // Assert
        Assert.True(notification.IsRead);
    }
}
