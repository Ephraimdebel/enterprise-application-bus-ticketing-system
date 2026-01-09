using Rating.Domain.Aggregates;
using Rating.Domain.ValueObjects;
using Xunit;

namespace Rating.UnitTests;

public class RatingTests
{
    [Fact]
    public void Constructor_ShouldCreateRating_WhenValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tripId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        var stars = new Score(5);
        var comment = "Excellent!";

        // Act
        var rating = new Rating.Domain.Aggregates.Rating(id, tripId, userId, targetId, stars, comment);

        // Assert
        Assert.Equal(id, rating.Id);
        Assert.Equal(tripId, rating.TripId);
        Assert.Equal(stars, rating.Stars);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTripIdIsEmpty()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new Rating.Domain.Aggregates.Rating(
            Guid.NewGuid(),
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Score(5),
            "Comment"));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenUserIdIsEmpty()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new Rating.Domain.Aggregates.Rating(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.Empty,
            Guid.NewGuid(),
            new Score(5),
            "Comment"));
    }
}
