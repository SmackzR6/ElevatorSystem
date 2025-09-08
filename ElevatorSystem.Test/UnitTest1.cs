using ElevatorAbs.Models;
using service.normalelevator.Model;

namespace ElevatorSystem.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Elevator_HandlesPassengerBoardingAndOffloading()
        {
            // Arrange
            var elevator = new NormalElevator { Id = 1 };
            var passenger = new Passenger(id: 101, destinationFloor: 5);

            // Act
            elevator.BoardPassenger(passenger);
            await elevator.MoveAsync();

            // Assert
            Assert.Equal(5, elevator.CurrentFloor); // Elevator should arrive at passenger's destination
            Assert.DoesNotContain(passenger, elevator.GetPassengers()); // Passenger should be offloaded
            Assert.Equal(ElevatorStatus.Idle, elevator.Status); // Elevator should be idle after completing request

            // Output for visual confirmation
            elevator.DisplayStatus();
        }
    }
}