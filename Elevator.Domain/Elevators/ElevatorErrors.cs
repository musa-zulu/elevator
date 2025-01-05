
using SharedKernel;

namespace Domain.Elevators;

public static class ElevatorErrors
{
    public static Error SomethingWentWrong(string message) => Error.Failure(
       "Elevators.Error",
       message);
}
