using Domain.Abstractions;
using Domain.Buildings;
using Domain.Elevators;
using NSubstitute;

namespace DomainTests.Buildings;

public class BuildingTests
{
    private readonly IElevatorFactory _mockElevatorFactory;

    public BuildingTests()
    {
        _mockElevatorFactory = Substitute.For<IElevatorFactory>();
    }

    [Fact]
    public void Constructor_ShouldInitializeFloorsAndElevators_Correctly()
    {
        // Arrange
        int numberOfFloors = 5;
        var elevatorConfigurations = new List<(ElevatorType Type, int MaxCapacity)>
        {
            (ElevatorType.Glass, 10),
            (ElevatorType.HighSpeed, 20)
        };

        _mockElevatorFactory.CreateElevator(ElevatorType.Glass, 1, 10).Returns(new GlassElevator(1, 10));
        _mockElevatorFactory.CreateElevator(ElevatorType.HighSpeed, 2, 20).Returns(new HighSpeedElevator(2, 20));

        // Act
        var building = new Building(numberOfFloors, elevatorConfigurations, _mockElevatorFactory);

        // Assert
        Assert.NotNull(building.Floors);
        Assert.Equal(numberOfFloors, building.Floors.Count);
        Assert.NotNull(building.Elevators);
        Assert.Equal(elevatorConfigurations.Count, building.Elevators.Count);

        // Verify that the floors are initialized correctly
        Assert.Equal(1, building.Floors.First().FloorNumber);
        Assert.Equal(numberOfFloors, building.Floors.Last().FloorNumber);

        // Verify that the elevators are initialized correctly
        Assert.IsType<GlassElevator>(building.Elevators[0]);
        Assert.IsType<HighSpeedElevator>(building.Elevators[1]);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenElevatorFactoryIsNull()
    {
        // Arrange
        int numberOfFloors = 5;
        var elevatorConfigurations = new List<(ElevatorType Type, int MaxCapacity)>
        {
            (ElevatorType.Glass, 10)
        };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Building(numberOfFloors, elevatorConfigurations, null!));
    }

    [Fact]
    public void GetFloor_ShouldReturnCorrectFloor_WhenFloorExists()
    {
        // Arrange
        int numberOfFloors = 3;
        var elevatorConfigurations = new List<(ElevatorType Type, int MaxCapacity)>
        {
            (ElevatorType.Freight, 15)
        };

        _mockElevatorFactory.CreateElevator(ElevatorType.Freight, 1, 15).Returns(new FreightElevator(1, 15));
        var building = new Building(numberOfFloors, elevatorConfigurations, _mockElevatorFactory);

        // Act
        var floor = building.GetFloor(2);

        // Assert
        Assert.NotNull(floor);
        Assert.Equal(2, floor.FloorNumber);
    }

    [Fact]
    public void GetFloor_ShouldThrowInvalidOperationException_WhenFloorDoesNotExist()
    {
        // Arrange
        int numberOfFloors = 3;
        var elevatorConfigurations = new List<(ElevatorType Type, int MaxCapacity)>
        {
            (ElevatorType.Glass, 10)
        };

        _mockElevatorFactory.CreateElevator(ElevatorType.Glass, 1, 10).Returns(new GlassElevator(1, 10));
        var building = new Building(numberOfFloors, elevatorConfigurations, _mockElevatorFactory);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => building.GetFloor(5));
        Assert.Equal("Floor 5 does not exist.", exception.Message);
    }    
}
