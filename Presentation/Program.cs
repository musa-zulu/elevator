using Application.Abstractions.Messaging;
using Application.Elevators.Commands;
using Domain.Buildings;
using Microsoft.Extensions.DependencyInjection;
using Presentation;

var serviceProvider = DependancyInjection.ConfigureServices();
var building = serviceProvider.GetService<Building>();
var handler = serviceProvider.GetService<ICommandHandler<ElevatorCommand>>();

while (true)
{
    DisplayElevatorStatus(building!);

    Console.WriteLine("Enter requested floor:");
    if (!int.TryParse(Console.ReadLine(), out int requestedFloor) || requestedFloor < 1)
    {
        Console.WriteLine("Invalid floor.");
        continue;
    }

    Console.WriteLine("Enter number of passengers:");
    if (!int.TryParse(Console.ReadLine(), out int passengers) || passengers < 1)
    {
        Console.WriteLine("Invalid passenger count.");
        continue;
    }

    var command = new ElevatorCommand(requestedFloor, passengers);

    var result = await handler!.Handle(command, CancellationToken.None);

    if (result.IsSuccess)
        Console.WriteLine("Elevator dispatched successfully.");
    else
        Console.WriteLine($"Error: {result.Error}");
}

static void DisplayElevatorStatus(Building building)
{
    Console.WriteLine("\nCurrent Elevator Status:");
    foreach (var elevator in building.Elevators)
    {
        Console.WriteLine($"Elevator {elevator.Id} ({elevator.ElevatorType}) [MaxCapacity: {elevator.MaxCapacity}]- Floor: {elevator.CurrentFloor}, Passengers: {elevator.PassengerCount}, Status: {(elevator.ElevatorStatus == Domain.Elevators.ElevatorStatus.Moving ? "Moving" : "Stationary")}.");
    }
    Console.WriteLine();
}
