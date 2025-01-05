namespace Domain.Elevators;

public sealed class FreightElevator(int id, int numberOfFloors)
    : Elevator(id, numberOfFloors, ElevatorType.Freight)
{
}
