using Elevator.Domain.Common;

namespace Elevator.Domain.Elevators;

public enum ElevatorStatus { Moving, Stopped, Idle, OutOfService }
public enum ElevatorDirection { Up, Down, Stationary }

public abstract class Elevator : Entity
{
    public int CurrentFloor { get; private set; }
    public ElevatorStatus ElevatorStatus { get; private set; }
    public int MaxCapacity { get; private set; }
    public int PassengerCount { get; private set; }
    public ElevatorDirection ElevatorDirection { get; private set; }

    protected Elevator(Guid id, int numberOfFloors, int maxCapacity)
        : base(id)
    {
        Reset();

        MaxCapacity = maxCapacity;
    }
    public void MoveToFloor(int targetFloor)
    {
        if (targetFloor <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(targetFloor), "Target floor must be greater than or equal to 1.");
        }

        if (targetFloor == CurrentFloor)
        {
            Console.WriteLine($"Elevator {Id} is already on floor {CurrentFloor}.");
            return;
        }

        SetDirection(targetFloor);
        ElevatorStatus = ElevatorStatus.Moving;

        Console.WriteLine($"Elevator {Id} is moving {ElevatorDirection} from Floor {CurrentFloor} to Floor {targetFloor}.");
    }

    public virtual void SetDirection(int targetFloor)
    {
        if (targetFloor > CurrentFloor)
        {
            ElevatorDirection = ElevatorDirection.Up;
        }
        else if (targetFloor < CurrentFloor)
        {
            ElevatorDirection = ElevatorDirection.Down;
        }
        else
        {
            ElevatorDirection = ElevatorDirection.Stationary;
            ElevatorStatus = ElevatorStatus.Idle;
        }
    }

    #region Helpers
    private void Reset()
    {
        CurrentFloor = 0;
        ElevatorStatus = ElevatorStatus.Idle;
        PassengerCount = 0;
        ElevatorDirection = ElevatorDirection.Stationary;
    }      
    #endregion
}
