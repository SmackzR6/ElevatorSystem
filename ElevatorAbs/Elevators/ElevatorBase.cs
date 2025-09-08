using ElevatorAbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorAbs.Elevators
{
    public abstract class ElevatorBase : IElevator
    {
        // identifies the elevator moving
        public  int Id { get;  set; }

        //tracking of current elevator 
        public  int CurrentFloor { get; protected set; }

        //indication of elevator status
        public  ElevatorStatus Status { get; protected set; } = ElevatorStatus.Idle;

        protected List<Passenger> Passengers = new();

        public int MaxCapacity { get; protected set; } = 5;

        public bool CanBoard => Passengers.Count < MaxCapacity;


        //Holding of lists of floors the elevator needs to visit.
        protected Queue<int> DestinationQueue = new();
      
        public virtual void AddDestination(int floor)
        {
            if(!DestinationQueue.Contains(floor))
            {
                DestinationQueue.Enqueue(floor);
            }
        }

        public virtual void BoardPassenger(Passenger passenger)
        {
            if (!CanBoard)
            {
                Console.WriteLine($"Elevator {Id} is full. Passenger {passenger.Id} cannot board.");
                return;
            }

            Passengers.Add(passenger);
            AddDestination(passenger.DestinationFloor);
        }

        public virtual void OffloadPassengers()
        {
            var offloaded = Passengers.Where(p => p.DestinationFloor == CurrentFloor).ToList();
            foreach (var passengeroff in offloaded)
            {
                Console.WriteLine($"Passenger {passengeroff.Id} offloaded at floor {CurrentFloor}");
                Passengers.Remove(passengeroff);
            }
        }

        public virtual IReadOnlyList<Passenger> GetPassengers() => Passengers.AsReadOnly();



        public abstract Task MoveAsync();
       

        public virtual void DisplayStatus()
        {
            Console.WriteLine($"Elevator {Id}at floor {CurrentFloor}, status:{Status}");
        }
    }
}
