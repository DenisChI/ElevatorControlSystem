using ElevatorControlSystem.Elevator;
using ElevatorControlSystem.Floor;

namespace ElevatorControlSystem
{
    public class Program
    {
        private const int MIN_FLOOR = 1;
        private const int MAX_FLOOR = 20;
        private static ObservableElevator[] _elevators = new ObservableElevator[2];
        private static Dictionary<int, ObserverFloor> _floors = new(MAX_FLOOR);

        static void InitializeObjects()
        {
            _elevators[0] = new ObservableElevator(MIN_FLOOR, MAX_FLOOR, cabinNumber: 1, currentFloor: 1, timeToGetPastOneFloor: 1);
            _elevators[1] = new ObservableElevator(MIN_FLOOR, MAX_FLOOR, cabinNumber: 2, currentFloor: 1, timeToGetPastOneFloor: 2);

            for (int i = MIN_FLOOR; i <= MAX_FLOOR; i++)
            {
                _floors[i] = new ObserverFloor(i);
                _elevators[0].AddObserver(_floors[i]);
                _elevators[1].AddObserver(_floors[i]);
            }
        }

        static ObservableElevator GetTheFastestElevatorCabin(ObservableElevator[] elevators, int destinationFloor)
        {
            Dictionary<ObservableElevator, double> times = new(elevators.Length);
            double time;
            for (int i = 0; i < elevators.Length; i++)
            {
                time = (double)(destinationFloor - elevators[i].CurrentFloor) / elevators[i].SecondsToGoOneFloor;
                times.Add(elevators[i], time);
            }
            ObservableElevator  fastest = times.MinBy(x => x.Value).Key;
            Console.WriteLine($"Доставку пассажира на {destinationFloor} этаж осуществит Лифт {fastest.CabinNumber}.");
            return fastest;
        }

        static void PassengerImitation(int passengerFloor, int destinationFloor)
        {
            Console.WriteLine($"Начало моделирования: {passengerFloor}->{destinationFloor}.");
            _floors[passengerFloor].PressCallButton();
            ObservableElevator fastest = GetTheFastestElevatorCabin(_elevators, passengerFloor);
            fastest.GoToFloor(passengerFloor);
            _floors[passengerFloor].CallButtonStatus = CallButtonState.NotCalled;

            fastest.PressFloorButton(destinationFloor);
            Console.WriteLine($"Завершение моделирования: {passengerFloor}->{destinationFloor}.");
        }

        static void Main(string[] args)
        {
            InitializeObjects();
            try
            {
                PassengerImitation(1, 14);
                PassengerImitation(15, 1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}