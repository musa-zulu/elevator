using Domain.Abstractions;
using Domain.Elevators;

namespace Application.Strategies;
internal sealed class NearestElevatorStrategy : IDispatchStrategy
{
    public async Task<Elevator> SelectElevatorAsync(IEnumerable<Elevator> elevators, int requestedFloor, int passengers)
    {
        return await Task.Run(() =>
            elevators
                .Where(e => e.CanLoadPassengers(passengers))
                .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                .FirstOrDefault()!);
    }
}
