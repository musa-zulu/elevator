using Application.Elevators.Commands;
using FluentAssertions;
using Xunit;

namespace ApplicationTests.Elevators.Commands;

public class ElevatorCommandTests
{
    [Fact]
    public void ElevatorCommand_Constructor_WithValidArguments_ShouldSetProperties()
    {
        // Arrange
        var targetFloor = 5;
        var passengersWaiting = 10;

        // Act
        var command = new ElevatorCommand(targetFloor, passengersWaiting);

        // Assert
        command.TargetFloor.Should().Be(targetFloor);
        command.PassengersWaiting.Should().Be(passengersWaiting);
    }
}