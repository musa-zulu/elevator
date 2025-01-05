using Application.Abstractions.Messaging;

namespace Application.Elevators.Commands;

public sealed record ElevatorCommand(int TargetFloor, int PassengersWaiting) : ICommand;