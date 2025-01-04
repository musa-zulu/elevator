namespace Domain.Elevators;

public sealed class HighSpeedElevator(int id, int numberOfFloors)
    : Elevator(id, numberOfFloors, ElevatorType.HighSpeed)
{
}
