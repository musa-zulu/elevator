using Application.Factories;
using Domain.Elevators;
using System;
using Xunit;

namespace ApplicationTests.Factories;

public class ElevatorFactoryTests
{
    private readonly ElevatorFactory _factory;

    public ElevatorFactoryTests()
    {
        _factory = new ElevatorFactory();
    }

    [Fact]
    public void CreateElevator_ShouldCreateGlassElevator_WhenTypeIsGlass()
    {
        // Arrange
        int id = 1;
        int maxCapacity = 1000;

        // Act
        var elevator = _factory.CreateElevator(ElevatorType.Glass, id, maxCapacity);

        // Assert
        Assert.IsType<GlassElevator>(elevator);
        Assert.Equal(id, elevator.Id);
        Assert.Equal(maxCapacity, elevator.MaxCapacity);
    }

    [Fact]
    public void CreateElevator_ShouldCreateFreightElevator_WhenTypeIsFreight()
    {
        // Arrange
        int id = 2;
        int maxCapacity = 2000;

        // Act
        var elevator = _factory.CreateElevator(ElevatorType.Freight, id, maxCapacity);

        // Assert
        Assert.IsType<FreightElevator>(elevator);
        Assert.Equal(id, elevator.Id);
        Assert.Equal(maxCapacity, elevator.MaxCapacity);
    }

    [Fact]
    public void CreateElevator_ShouldCreateHighSpeedElevator_WhenTypeIsHighSpeed()
    {
        // Arrange
        int id = 3;
        int maxCapacity = 1500;

        // Act
        var elevator = _factory.CreateElevator(ElevatorType.HighSpeed, id, maxCapacity);

        // Assert
        Assert.IsType<HighSpeedElevator>(elevator);
        Assert.Equal(id, elevator.Id);
        Assert.Equal(maxCapacity, elevator.MaxCapacity);
    }

    [Fact]
    public void CreateElevator_ShouldThrowArgumentException_WhenTypeIsUnknown()
    {
        // Arrange
        int id = 4;
        int maxCapacity = 1000;
        var unknownType = (ElevatorType)999;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _factory.CreateElevator(unknownType, id, maxCapacity));

        Assert.Equal($"Unknown elevator type: {unknownType}", exception.Message);
    }
}

