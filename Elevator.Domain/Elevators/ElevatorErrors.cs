
using SharedKernel;

namespace Domain.Elevators;

public static class ElevatorErrors
{
    public static Error NotAvailable() => Error.NotAvailable(
        "Elevators.NotAvailable",
        $"No available elevators. Please wait...");

    public static Error MaxCapacityReached() => Error.Failure(
        "Elevators.MaxCapicityReached",
        $"Maximum capacity reached. Others must use other available elevators.");

    public static Error SomethingWentWrong(string message) => Error.Failure(
       "Elevators.Error",
       message);
}
