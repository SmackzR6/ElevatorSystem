using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorAbs.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public int DestinationFloor { get; set; }

        public Passenger(int id, int destinationFloor)
        {
            Id = id;
            DestinationFloor = destinationFloor;
        }

        public override string ToString()
        {
            return $"Passenger {Id} → Floor {DestinationFloor}";
        }

    }
}
