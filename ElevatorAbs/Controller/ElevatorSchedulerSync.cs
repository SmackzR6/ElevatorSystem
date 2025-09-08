using System;
using ElevatorAbs.Models;
using ElevatorAbs.Elevators;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorAbs.Controller
{
    public class ElevatorSchedulerSync
    {
        public IElevator AssignElevator(ElevatorRequest request, List<IElevator> elevators)
        {
            //Prioritize idle elevators closest to the requested floor
            var idleElevators = elevators
                                .Where(e => e.Status == ElevatorStatus.Idle)
                                .OrderBy(e => Math.Abs(e.CurrentFloor - request.RequestedFloor))
                                .ToList();

            //Return the closest idle elevator if available
            if (idleElevators.Any())
            {
                return idleElevators.First();

            }
            else
            {
                //Fallback: return the closest elevator regardless of status
                return elevators.OrderBy(e => Math.Abs(e.CurrentFloor - request.RequestedFloor)).FirstOrDefault() ?? throw new InvalidOperationException();
            }

        }
    }
}
