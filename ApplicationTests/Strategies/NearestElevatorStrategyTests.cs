using Application.Strategies;
using Domain.Elevators;
using Xunit;

namespace ApplicationTests.Strategies;

public class NearestElevatorStrategyTests
{
    private readonly NearestElevatorStrategy _strategy;

    public NearestElevatorStrategyTests()
    {
        _strategy = new NearestElevatorStrategy();
    }

    [Fact]
    public async Task SelectElevatorAsync_ShouldReturnNearestElevator_WhenMultipleElevatorsAvailable()
    {
        // Arrange
        var elevators = new List<Elevator>
        {
            new HighSpeedElevator(1, 1000) { CurrentFloor = 6 },
            new FreightElevator(2, 1000) { CurrentFloor = 3 },
            new GlassElevator(3, 1000) { CurrentFloor = 8 }
        };

        int requestedFloor = 4;
        int passengers = 5;

        // Act
        var result = await _strategy.SelectElevatorAsync(elevators, requestedFloor, passengers);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id); // Elevator at floor 3 is nearest to floor 4
    }

    [Fact]
    public async Task SelectElevatorAsync_ShouldReturnNull_WhenNoElevatorCanLoadPassengers()
    {
        // Arrange
        var elevators = new List<Elevator>
        {
            new HighSpeedElevator(1, 1000) { CurrentFloor = 5, PassengerCount = 1000 },
            new FreightElevator(2, 1000) { CurrentFloor = 3, PassengerCount = 1000 },
        };

        int requestedFloor = 3;
        int passengers = 5;

        // Act
        var result = await _strategy.SelectElevatorAsync(elevators, requestedFloor, passengers);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SelectElevatorAsync_ShouldReturnNearestElevator_WhenElevatorsHaveDifferentCapacities()
    {
        // Arrange
        var elevators = new List<Elevator>
        {
            new HighSpeedElevator(1, 5) { CurrentFloor = 1 }, // Cannot load 10 passengers
            new FreightElevator(2, 15) { CurrentFloor = 7 }, // Can load 10 passengers
            new GlassElevator(3, 20) { CurrentFloor = 3 } // Can load 10 passengers
        };

        int requestedFloor = 4;
        int passengers = 10;

        // Act
        var result = await _strategy.SelectElevatorAsync(elevators, requestedFloor, passengers);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Id); // Elevator at floor 3 is nearest and can load passengers
    }

    [Fact]
    public async Task SelectElevatorAsync_ShouldHandleEmptyElevatorList_Gracefully()
    {
        // Arrange
        var elevators = new List<Elevator>();
        int requestedFloor = 4;
        int passengers = 5;

        // Act
        var result = await _strategy.SelectElevatorAsync(elevators, requestedFloor, passengers);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SelectElevatorAsync_ShouldReturnNearestElevator_WhenElevatorsAreOnTheSameFloor()
    {
        // Arrange
        var elevators = new List<Elevator>
        {
            new HighSpeedElevator(1, 1000) { CurrentFloor = 5 },
            new FreightElevator(2, 1000) { CurrentFloor = 5 }
        };

        int requestedFloor = 5;
        int passengers = 10;

        // Act
        var result = await _strategy.SelectElevatorAsync(elevators, requestedFloor, passengers);

        // Assert
        Assert.NotNull(result);
        Assert.True(elevators.Contains(result)); // Any elevator at floor 5 is acceptable
    }
}