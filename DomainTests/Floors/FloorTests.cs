using Domain.Floors;

namespace DomainTests.Floors;

public class FloorTests
{
    [Fact]
    public void Constructor_ShouldInitializeFloorWithCorrectNumber()
    {
        // Arrange
        int floorNumber = 3;

        // Act
        var floor = new Floor(floorNumber);

        // Assert
        Assert.Equal(floorNumber, floor.FloorNumber);
        Assert.Equal(0, floor.PassengerCount);
        Assert.Empty(floor.PassengerQueue);
    }

    [Fact]
    public void AddPassengers_ShouldAddPassengersToQueue()
    {
        // Arrange
        var floor = new Floor(1);
        int passengerCount = 5;

        // Act
        floor.AddPassengers(passengerCount);

        // Assert
        Assert.Single(floor.PassengerQueue);
        Assert.Equal(passengerCount, floor.PassengerQueue.Peek());
    }

    [Fact]
    public void AddPassengers_ShouldThrowArgumentException_WhenPassengerCountIsNegative()
    {
        // Arrange
        var floor = new Floor(1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => floor.AddPassengers(-5));
    }

    [Fact]
    public void ResetPassengerCount_ShouldSetPassengerCountToZero()
    {
        // Arrange
        var floor = new Floor(1);
        floor.AddPassengers(5);

        // Act
        floor.ResetPassengerCount();

        // Assert
        Assert.Equal(0, floor.PassengerCount);
    }

    [Fact]
    public void RemovePassengerFromQueue_ShouldRemoveAndReturnFirstPassengerCount()
    {
        // Arrange
        var floor = new Floor(1);
        floor.AddPassengers(5);
        floor.AddPassengers(10);

        // Act
        var removedPassengerCount = floor.RemovePassengerFromQueue();

        // Assert
        Assert.Equal(5, removedPassengerCount);
        Assert.Single(floor.PassengerQueue);
        Assert.Equal(10, floor.PassengerQueue.Peek());
    }

    [Fact]
    public void RemovePassengerFromQueue_ShouldThrowInvalidOperationException_WhenQueueIsEmpty()
    {
        // Arrange
        var floor = new Floor(1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => floor.RemovePassengerFromQueue());
    }
}

