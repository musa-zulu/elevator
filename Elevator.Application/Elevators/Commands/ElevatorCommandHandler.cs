using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Buildings;
using Domain.Elevators;
using SharedKernel;

namespace Application.Elevators.Commands;

internal sealed class ElevatorCommandHandler(Building building, IDispatchStrategy dispatchStrategy)
    : ICommandHandler<ElevatorCommand>
{
    public async Task<Result> Handle(ElevatorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var floor = building.GetFloor(request.TargetFloor);

            floor.AddPassengers(request.PassengersWaiting);

            var elevator = await dispatchStrategy.SelectElevatorAsync(building.Elevators, request.TargetFloor, request.PassengersWaiting);

            if (elevator == null)
                return Result.Failure(ElevatorErrors.SomethingWentWrong($"No elevator found for floor {request.TargetFloor} with {request.PassengersWaiting} passengers"));

            elevator.LoadPassengers(request.PassengersWaiting);

            await elevator.MoveToFloorAsync(request.TargetFloor, status =>
            {
                Console.WriteLine($"[Movement - Update] : {status}");
            });

            elevator.UnloadPassengers(request.PassengersWaiting);
            floor.RemovePassengerFromQueue();
            floor.ResetPassengerCount();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ElevatorErrors.SomethingWentWrong(ex.Message));
        }
    }
}
