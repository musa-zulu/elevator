using Xunit;
using NSubstitute;
using Domain.Floors;
using Domain.Buildings;
using Domain.Elevators;
using Domain.Abstractions;
using Application.Elevators.Commands;

namespace ApplicationTests.Elevators.Commands;

public class ElevatorCommandHandlerTests
{
    private readonly Building _mockBuilding;
    private readonly Floor _mockFloor;
    private readonly Elevator _mockElevator;
    private readonly IDispatchStrategy _mockDispatchStrategy;
    private readonly ElevatorCommandHandler _handler;

    public ElevatorCommandHandlerTests()
    {
        var elevatorFactory = Substitute.For<IElevatorFactory>();

        var elevatorConfigurations = new List<(ElevatorType Type, int MaxCapacity)>
                {
                    (ElevatorType.Glass, 8),
                    (ElevatorType.Freight, 15),
                    (ElevatorType.HighSpeed, 10)
                };

        _mockFloor = Substitute.For<Floor>(5);

        _mockDispatchStrategy = Substitute.For<IDispatchStrategy>();

        _mockElevator = Substitute.For<Elevator>(1, 10, ElevatorType.Glass);

        _mockBuilding = Substitute.For<Building>(10, elevatorConfigurations, elevatorFactory);

        _mockDispatchStrategy.SelectElevatorAsync(_mockBuilding.Elevators, 5, 3)
                             .Returns(Task.FromResult(_mockElevator));

        _handler = new ElevatorCommandHandler(_mockBuilding, _mockDispatchStrategy);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenElevatorHandlesRequestSuccessfully()
    {
        // Arrange
        var targetFloor = 5;
        var passengersWaiting = 3;
        var floor = new Floor(targetFloor);
        // Act
        var result = await _handler.Handle(new ElevatorCommand(targetFloor, passengersWaiting), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(floor.PassengerQueue);
        await _mockElevator.Received(1).MoveToFloorAsync(targetFloor, status =>
        {
            Console.WriteLine($"[Movement - Update] : {status}");
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFloorDoesNotExist()
    {
        // Arrange
        var invalidTargetFloor = 99;
        _mockBuilding.GetFloor(invalidTargetFloor).Returns(x => throw new ArgumentException($"Floor {invalidTargetFloor} does not exist."));

        // Act
        var result = await _handler.Handle(new ElevatorCommand(invalidTargetFloor, 3), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Floor {invalidTargetFloor} does not exist.", result.Error.Description);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoElevatorIsAvailable()
    {
        // Arrange
        var targetFloor = 5;
        var passengersWaiting = 4;
      
        _mockBuilding.GetFloor(targetFloor).Returns(_mockFloor);

        _mockDispatchStrategy.SelectElevatorAsync(_mockBuilding.Elevators, targetFloor, passengersWaiting)
            .Returns(Task.FromResult<Elevator>(null!));

        // Act
        var result = await _handler.Handle(new ElevatorCommand(targetFloor, passengersWaiting), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ElevatorErrors.NotAvailable().Description, result.Error.Description);
    }

    [Fact]
    public async Task Handle_ShouldHandleElevatorLoadAndUnloadProperly()
    {
        // Arrange
        var targetFloor = 3;
        var passengersWaiting = 2;       

        _mockBuilding.GetFloor(targetFloor).Returns(_mockFloor);

        _mockDispatchStrategy.SelectElevatorAsync(_mockBuilding.Elevators, targetFloor, passengersWaiting)
            .Returns(Task.FromResult(_mockElevator));

        // Act
        var result = await _handler.Handle(new ElevatorCommand(targetFloor, passengersWaiting), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _mockElevator.Received(1).LoadPassengers(passengersWaiting);
        _mockElevator.Received(1).UnloadPassengers(passengersWaiting);
        Assert.Empty(_mockFloor.PassengerQueue);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
    {
        // Arrange
        var targetFloor = 3;
        _mockBuilding.GetFloor(targetFloor).Returns(x => throw new InvalidOperationException("Unexpected error"));

        // Act
        var result = await _handler.Handle(new ElevatorCommand(targetFloor, 3), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Unexpected error", result.Error.Description);
        Assert.Contains("Unexpected error", result.Error.Description);
    }
}