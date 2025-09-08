using ElevatorAbs.Elevators;
using ElevatorAbs.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorAbs.Controller
{
    public class CentralElevatorSync
    {
        //Holds the list of elevators in the building
        public List<IElevator> Elevators { get;}

        //Queue of incoming elevator requests
        private Queue<ElevatorRequest> requestQue = new Queue<ElevatorRequest>();

        //Scheduler responsible for assigning elevators to requests
        private ElevatorSchedulerSync scheduler = new ElevatorSchedulerSync();

        public CentralElevatorSync(List<IElevator> elevators) 
        {
          Elevators = elevators;
        }

        // Enqueue a new elevator request
        public void SubmitRequest(ElevatorRequest request)
        {
            requestQue.Enqueue(request);
        }

        //Process all queued requests and coordinate elevator movement
        public async Task CoordinateAsync()
        {
            while (requestQue.Count > 0)
            {
                //Dequeue the next request
                var request = requestQue.Dequeue();

                //Assign an elevator to the request
                var elevator = scheduler.AssignElevator(request, Elevators);
                if (elevator is ElevatorBase eb && request.Passenger != null)
                {
                    //Attempt to board the passenger if capacity allows
                    if (eb.CanBoard)
                    {
                        eb.BoardPassenger(request.Passenger);
                        Console.WriteLine($"Passenger {request.Passenger.Id} boarded Elevator {eb.Id} → Destination: {request.Passenger.DestinationFloor}");
                    }
                    else
                    {
                        Console.WriteLine($"Elevator {eb.Id} is full. Trying to assign another elevator...");
                        TryAssignToAnotherElevator(request);
                    }
                }
                else
                {
                    //No passenger attached — treat as a floor call
                    elevator?.AddDestination(request.RequestedFloor);
                }


            }

            var tasks = Elevators.Select(e => e.MoveAsync());
            await Task.WhenAll(tasks);
           
        }

        // Attempt to reassign a passenger to another available elevator
        private void TryAssignToAnotherElevator(ElevatorRequest request)
        {
            var availableElevators = Elevators
            .Where(e => e is ElevatorBase eb && eb.CanBoard)
            .ToList();

            var fallback = scheduler.AssignElevator(request, availableElevators);
            if (fallback is ElevatorBase eb)
            {
                var passenger = request.Passenger ?? throw new InvalidOperationException("Passenger is required for boarding.");
                eb.BoardPassenger(passenger);
                Console.WriteLine($"Passenger {passenger.Id} boarded fallback Elevator {eb.Id}");
            }
            else
            {
                //No elevator available — defer the request
                if (request.Passenger != null)
                {
                    Console.WriteLine($"No available elevators for Passenger {request.Passenger.Id}. Request deferred.");
                }
                else
                {
                    Console.WriteLine($"No available elevators for request from floor {request.RequestedFloor} going {request.Direction}. Request deferred.");
                }

                requestQue.Enqueue(request); //  retry later
            }

        }
    }
}
