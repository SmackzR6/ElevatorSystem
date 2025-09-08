using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorAbs.Models
{
    public class ElevatorRequest
    {
        public int RequestedFloor { get; }
        public Direction Direction { get; }
        public Passenger? Passenger { get; }


        public ElevatorRequest(int requestedFloor, Direction direction, Passenger? passenger = null)
        {  
            RequestedFloor = requestedFloor;
            Direction = direction;
            Passenger = passenger;

        }


        public override string ToString()
        {
            if (Passenger != null)
            {
                return $"Request from floor {RequestedFloor} going {Direction} for Passenger {Passenger.Id} → {Passenger.DestinationFloor}";
            }
            return $"Request from floor {RequestedFloor} going {Direction}";
        }

    }
}
