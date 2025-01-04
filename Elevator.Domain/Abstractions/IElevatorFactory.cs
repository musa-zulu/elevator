using Domain.Elevators;

namespace Domain.Abstractions;

public interface IElevatorFactory
{
    Elevator CreateElevator(ElevatorType type, int id, int maxCapacity);
}
