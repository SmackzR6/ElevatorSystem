using ElevatorAbs.Elevators;
using ElevatorAbs.Models;

namespace service.normalelevator.Model
{
    public class NormalElevator:ElevatorBase
    {
        public override async Task MoveAsync()
        {
            if (DestinationQueue.Count == 0)
            {
                Status = ElevatorStatus.Idle;
                return;
            }

            int target = DestinationQueue.Peek();
            Status = target > CurrentFloor ? ElevatorStatus.MovingUp : ElevatorStatus.MovingDown;

            int step = target > CurrentFloor ? 1 : -1;

            while (CurrentFloor != target)
            {
                await Task.Delay(1000); //simulation of movements
                CurrentFloor += step;
                Console.WriteLine($" Elevator {Id} at floor {CurrentFloor}");
            }

            DestinationQueue.Dequeue();

            //Offload passengers at arrival
            OffloadPassengers();

            Status = DestinationQueue.Count == 0 ? ElevatorStatus.Idle : ElevatorStatus.Moving;
            Console.WriteLine($" Elevator {Id} arrived at floor {CurrentFloor}");
            DisplayStatus();
            await Task.Delay(1000);
        }

        public override void DisplayStatus()
        {
            Console.WriteLine($" Elevator {Id} (normal) at floor {CurrentFloor}, status: {Status}, passengers: {Passengers.Count}");
        }
    }
}
