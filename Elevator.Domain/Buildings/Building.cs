using Domain.Abstractions;
using Domain.Elevators;
using Domain.Floors;
using SharedKernel;

namespace Domain.Buildings;

public sealed class Building : Entity
{
    public List<Elevator> Elevators { get; }
    public List<Floor> Floors { get; }

    public Building(int numberOfFloors, List<(ElevatorType Type, int MaxCapacity)> elevatorConfigurations, IElevatorFactory elevatorFactory)
    {
        if (elevatorFactory == null)
        {
            throw new ArgumentNullException(nameof(elevatorFactory));
        }

        Floors = Enumerable
            .Range(1, numberOfFloors)
            .Select(f => new Floor(f)).ToList();

        Elevators = elevatorConfigurations
            .Select((config, index) =>
            elevatorFactory.CreateElevator(config.Type, (index + 1), config.MaxCapacity)
        ).ToList();
    }

    public Floor GetFloor(int floorNumber) =>
        Floors.Find(f => f.FloorNumber == floorNumber)
        ?? throw new InvalidOperationException($"Floor {floorNumber} does not exist.");
}
