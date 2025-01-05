using Application;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;
public static class DependancyInjection
{
    public static IServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddElevatorServices();

        return serviceCollection.BuildServiceProvider();
    }

}
