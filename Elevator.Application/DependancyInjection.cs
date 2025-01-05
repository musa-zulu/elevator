using Application.Abstractions.Messaging;
using Application.Elevators.Commands;
using Application.Factories;
using Application.Strategies;
using Domain.Abstractions;
using Domain.Buildings;
using Domain.Elevators;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddElevatorServices(this IServiceCollection services)
    {
        services.AddSingleton<IElevatorFactory, ElevatorFactory>()
            .AddSingleton<IDispatchStrategy, NearestElevatorStrategy>()
            .AddSingleton(provider =>
            {
                var elevatorFactory = provider.GetRequiredService<IElevatorFactory>();
                var elevatorConfigurations = new List<(ElevatorType Type, int MaxCapacity)>
                {
                    (ElevatorType.Glass, 8),
                    (ElevatorType.Freight, 15),
                    (ElevatorType.HighSpeed, 10)
                };

                return new Building(numberOfFloors: 10, elevatorConfigurations, elevatorFactory);
            })
            .AddSingleton<ICommandHandler<ElevatorCommand>, ElevatorCommandHandler>();
        return services;
    }
}
