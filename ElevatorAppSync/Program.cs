using ElevatorAbs.Models;
using ElevatorAbs.Elevators;
using ElevatorAbs.Controller;
using ElevatorAbs;
using service.normalelevator.Model;

namespace ElevatorAppSync
{
    internal class Program
    {
        static async Task Main(string[] args)
        {


            /****
             * Initialize the building with 3 elevators
             * Visual layout: [1][2][3]
             * */
            var elevators = new List<IElevator>
          {
              new NormalElevator{ Id = 1},
              new NormalElevator{ Id = 2},
              new NormalElevator{ Id = 3}

          };


            //Central controller that coordinates elevator assignments and movement
            var controller = new  CentralElevatorSync(elevators);

            //Static counter to assign unique IDs to passengers
            int passengerIdCounter = 100;


            while (true)
            {
                Console.Clear();

                //Display elevator positions across all floors
                DisplayBuildingStatus(elevators);

                Console.WriteLine("Enter request (e.g '5 up' or 'down' without single quotes) or 'exit': ");


                //Exit condition
                var input = Console.ReadLine();
                if (input?.ToLower() == "exit")
                {
                    break;
                }

                //Parse input into floor and direction
                var parts = input?.Split(' ');

                if (parts?.Length !=2 || !int.TryParse(parts[0], out int floor))
                {
                    continue;
                }

                //Determine direction
                Direction drc = parts[1].ToLower() == "up" ? Direction.Up : Direction.Down;

                //Prompt for number of passengers
                Console.Write("Enter number of passengers: ");
                if (!int.TryParse(Console.ReadLine(), out int passengerCount) || passengerCount <= 0)
                {
                    Console.WriteLine("Invalid passenger count.");
                    await Task.Delay(1000);
                    continue;
                }

                //Create and submit individual passenger requests
                for (int i = 0; i < passengerCount; i++)
                {
                    // Simple logic: assume each passenger wants to go one floor in the requested direction
                    int destination = drc == Direction.Up ? floor + 1 : floor - 1;
                    var passenger = new Passenger(passengerIdCounter++, destination);

                    // var request = new ElevatorRequest(floor, drc, passenger);

                    controller.SubmitRequest(new ElevatorRequest(floor, drc, passenger));
                }



                // controller.SubmitRequest(new ElevatorRequest(floor, drc));
                // Coordinate elevator movement and handle requests
                await controller.CoordinateAsync();
                await Task.Delay(1000);
                
              
            }
        }

        private static void DisplayBuildingStatus(List<IElevator> elevators)
        {
            // Render building floors from top (10) to ground (0)
            for (int floor = 10; floor >= 0; floor--)
            {
              Console.WriteLine($"Floor {floor,2} |");
                foreach (var elevator in elevators)
                {
                    string marker = elevator.CurrentFloor == floor ? $"({elevator.Id})" : "()";
                    Console.WriteLine(marker + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
