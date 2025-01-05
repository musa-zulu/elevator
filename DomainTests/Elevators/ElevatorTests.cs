using Domain.Elevators;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DomainTests.Elevators;

public class ElevatorTests
{
    private sealed class TestElevator : Elevator
    {
        public TestElevator(int id, int maxCapacity, ElevatorType elevatorType)
            : base(id, maxCapacity, elevatorType) { }
    }

    [Fact]
    public async Task MoveToFloorAsync_ShouldMoveToTargetFloor()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.HighSpeed);
        int targetFloor = 5;
        var statusUpdates = new List<string>();

        // Act
        await elevator.MoveToFloorAsync(targetFloor, status => statusUpdates.Add(status));

        // Assert
        Assert.Equal(targetFloor, elevator.CurrentFloor);
        Assert.Contains(statusUpdates, s => s.Contains($"Elevator [1] has reached Floor [{targetFloor}]"));
    }

    [Fact]
    public async Task MoveToFloorAsync_ShouldHandleAlreadyOnTargetFloor()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.Freight);
        var statusUpdates = new List<string>();

        // Act
        await elevator.MoveToFloorAsync(elevator.CurrentFloor, status => statusUpdates.Add(status));

        // Assert
        Assert.Contains(statusUpdates, s => s.Contains($"Elevator [1] is already on Floor [1]."));
    }

    [Fact]
    public async Task MoveToFloorAsync_ShouldThrowForInvalidFloor()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.Glass);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => elevator.MoveToFloorAsync(-1, null));
    }

    [Fact]
    public void LoadPassengers_ShouldIncreasePassengerCount()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.Freight);

        // Act
        elevator.LoadPassengers(5);

        // Assert
        Assert.Equal(5, elevator.PassengerCount);
    }

    [Fact]
    public void LoadPassengers_ShouldThrowIfExceedsCapacity()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.Glass);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => elevator.LoadPassengers(15));
    }

    [Fact]
    public void LoadPassengers_ShouldThrowForNegativePassengers()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.Freight);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => elevator.LoadPassengers(-1));
    }

    [Fact]
    public void UnloadPassengers_ShouldDecreasePassengerCount()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.HighSpeed);
        elevator.LoadPassengers(5);

        // Act
        elevator.UnloadPassengers(3);

        // Assert
        Assert.Equal(2, elevator.PassengerCount);
    }

    [Fact]
    public void UnloadPassengers_ShouldNotGoBelowZero()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.Freight);
        elevator.LoadPassengers(3);

        // Act
        elevator.UnloadPassengers(5);

        // Assert
        Assert.Equal(0, elevator.PassengerCount);
    }

    [Fact]
    public void CanLoadPassengers_ShouldReturnTrueIfWithinCapacity()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.Freight);

        // Act
        var result = elevator.CanLoadPassengers(5);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanLoadPassengers_ShouldThrowForNegativePassengers()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.HighSpeed);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => elevator.CanLoadPassengers(-1));
    }

    [Fact]
    public void CanLoadPassengers_ShouldReturnFalseIfExceedsCapacity()
    {
        // Arrange
        var elevator = new TestElevator(1, 10, ElevatorType.Glass);

        // Act
        var result = elevator.CanLoadPassengers(15);

        // Assert
        Assert.False(result);
    }
}
