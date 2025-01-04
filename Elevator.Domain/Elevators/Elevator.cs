using SharedKernel;

namespace Domain.Elevators;

public abstract class Elevator(int id, int maxCapacity, ElevatorType elevatorType)
    : Entity(id)
{
    public int MaxCapacity { get; } = maxCapacity;
    public ElevatorType ElevatorType { get; } = elevatorType;
    public int CurrentFloor { get; private set; } = 1;
    public int PassengerCount { get; private set; } = 0;
    public ElevatorStatus ElevatorStatus { get; private set; } = ElevatorStatus.Idle;
    public ElevatorDirection ElevatorDirection { get; private set; } = ElevatorDirection.Stationary;

    public async Task MoveToFloorAsync(int targetFloor, Action<string> statusCallback)
    {
        if (Invalid(targetFloor))
        {
            throw new ArgumentOutOfRangeException(nameof(targetFloor), "Target floor must be greater than or equal to 1.");
        }

        if (AlreadyOn(targetFloor))
        {
            statusCallback($"Elevator [{Id}] is already on Floor [{CurrentFloor}].");
            return;
        }

        await ElevatorMovementBetweenFloors(targetFloor, statusCallback);
    }

    public void LoadPassengers(int passengers)
    {
        if (passengers < 0 || !CanLoadPassengers(passengers))
            throw new InvalidOperationException("Invalid passengers count. Passengers cannot be negative or exceed maximum capacity.");

        PassengerCount += passengers;
    }

    public bool CanLoadPassengers(int passengers) => (PassengerCount + passengers) <= MaxCapacity;
    public void UnloadPassengers(int passengers) => PassengerCount = Math.Max(0, PassengerCount - passengers);

    #region Private Methods
    private static bool Invalid(int targetFloor) => targetFloor <= 0;
    private bool NotOn(int targetFloor) => CurrentFloor != targetFloor;
    private bool AlreadyOn(int targetFloor) => targetFloor == CurrentFloor;
    private void SetElevatorDirection(int targetFloor)
        => ElevatorDirection = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

    private async Task ElevatorMovementBetweenFloors(int targetFloor, Action<string> statusCallback)
    {
        SetElevatorDirection(targetFloor);

        while (NotOn(targetFloor))
        {
            CurrentFloor += targetFloor > CurrentFloor ? 1 : -1;
            statusCallback?.Invoke($"Elevator [{Id}] is on floor [{CurrentFloor}] moving {ElevatorDirection}.");
            await Task.Delay(3000);
        }

        ElevatorDirection = ElevatorDirection.Stationary;
        ElevatorStatus = ElevatorStatus.Stopped;
        statusCallback?.Invoke($"Elevator [{Id}] has reached Floor [{targetFloor}] and is now stationary.");
    }
    #endregion
}
