# Elevator Simulation Console App
 A modular, event-driven simulation of a building with multiple elevators. Supports passenger requests, direction handling, capacity limits, and dynamic elevator coordination.

# Features
- Simulates a building with multiple elevators
- Handles user input for floor, direction, and passenger count
- Assigns elevators dynamically based on availability
- Enforces passenger capacity limits
- Supports fallback elevator assignment and deferred requests
- Displays elevator positions across floors in real time

# Architecture Overview
- ElevatorBase: Abstract class with movement, capacity, and passenger logic
- NormalElevator: Concrete implementation of a standard elevator
- Passenger: Represents a rider with a unique ID and destination
- ElevatorRequest: Encapsulates floor, direction, and optional passenger
- CentralElevatorSync: Coordinates elevator assignments and movement
- ElevatorSchedulerSync: Strategy for selecting the best elevator
- Main Loop: Console interface for user input and simulation control

# How to Use
- Run the app in a console environment
- Enter a request in the format: 5 up or 3 down
- Enter the number of passengers for that request
- Watch elevators move and offload passengers
- Type exit to quit the simulation

# Sample Test
[Fact]
public async Task Elevator_HandlesPassengerBoardingAndOffloading()
{
    var elevator = new NormalElevator { Id = 1 };
    var passenger = new Passenger(101, 5);

    elevator.BoardPassenger(passenger);
    await elevator.MoveAsync();

    Assert.Equal(5, elevator.CurrentFloor);
    Assert.DoesNotContain(passenger, elevator.GetPassengers());
    Assert.Equal(ElevatorStatus.Idle, elevator.Status);
}

# Future Enhancements
  extend and add new type elevators
- Freight elevator
- High speed elevator
- Glass elevator
- Real Visualisation (using spectre.console ui)