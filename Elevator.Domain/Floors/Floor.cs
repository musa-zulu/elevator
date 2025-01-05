namespace Domain.Floors;

public class Floor(int floorNumber)
{
    public int PassengerCount { get; private set; } = 0;
    public int FloorNumber { get; } = floorNumber;
    public Queue<int> PassengerQueue { get; private set; } = new Queue<int>();
    
    public void AddPassengers(int passengerCount)
    {
        if (passengerCount < 0) throw new ArgumentException("Cannot add negative passengers.");
        PassengerQueue.Enqueue(passengerCount);
    }
    public void ResetPassengerCount() => PassengerCount = 0;
    public int RemovePassengerFromQueue() => PassengerQueue.Dequeue();
}
