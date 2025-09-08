using ElevatorAbs.Models;

namespace ElevatorAbs
{
    public interface IElevator
    {
        int Id { get; }
        int CurrentFloor { get; }
        
        ElevatorStatus Status { get; }

        void AddDestination(int floor);
        Task MoveAsync();
        void DisplayStatus();

        //Passenger operations (BoardPassenger, OffloadPassengers)

        void BoardPassenger(Passenger passenger);
        void OffloadPassengers();
        IReadOnlyList<Passenger> GetPassengers();

       // Capacity checker(CanBoard)

        bool CanBoard { get; }

    }
}
