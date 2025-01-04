namespace Domain.Elevators;

public sealed class GlassElevator(int id, int numberOfFloors)
    : Elevator(id, numberOfFloors, ElevatorType.Glass)
{
}
