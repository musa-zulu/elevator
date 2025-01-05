using Domain.Elevators;

namespace Domain.Abstractions;

public interface IDispatchStrategy
{
    Task<Elevator> SelectElevatorAsync(IEnumerable<Elevator> elevators, int requestedFloor, int passengers);
}
