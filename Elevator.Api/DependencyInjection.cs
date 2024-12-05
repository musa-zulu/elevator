namespace Elevator.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();        
        services.AddProblemDetails();

        return services;
    }
}