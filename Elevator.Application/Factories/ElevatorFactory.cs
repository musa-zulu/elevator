using Domain.Abstractions;
using Domain.Elevators;

namespace Application.Factories;

internal sealed class ElevatorFactory : IElevatorFactory
{
    public Elevator CreateElevator(ElevatorType type, int id, int maxCapacity)
    {
        return type switch
        {
            ElevatorType.Glass => new GlassElevator(id, maxCapacity),
            ElevatorType.Freight => new FreightElevator(id, maxCapacity),
            ElevatorType.HighSpeed => new HighSpeedElevator(id, maxCapacity),
            _ => throw new ArgumentException($"Unknown elevator type: {type}")
        };
    }
}