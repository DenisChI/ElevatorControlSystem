using ElevatorControlSystem.Floor;

namespace ElevatorControlSystem.Elevator
{
    class ObservableElevator : IObservable
    {

        private int _currentFloor;
        private ElevatorState _state;
        private List<IObserver> _observers;
        private readonly int _minFloor;
        private readonly int _maxFloor;

        public int CurrentFloor
        {
            get { return _currentFloor; }
            private set
            {
                _currentFloor = value;
                NotifyObservers();
            }
        }
        public ElevatorState State
        {
            get { return _state; }
            private set
            {
                _state = value;
                NotifyObservers();
            }
        }

        public int CabinNumber { get; private set; }
        public int SecondsToGoOneFloor { get; private set; }

        public ObservableElevator(int minFloor, int maxFloor, int cabinNumber, int currentFloor, int timeToGetPastOneFloor)
        {
            if (currentFloor < minFloor || currentFloor > maxFloor)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentFloor),
                    currentFloor,
                    $"Указан не верный этаж. Диапазон допустимых этажей: {_minFloor}-{_maxFloor}.");
            }
            _minFloor = minFloor;
            _maxFloor = maxFloor;
            _observers = new List<IObserver>();
            CabinNumber = cabinNumber;
            CurrentFloor = currentFloor;
            SecondsToGoOneFloor = timeToGetPastOneFloor;
        }

        private int GoFloorUp() => CurrentFloor++;
        private int GoFloorDown() => CurrentFloor--;

        private delegate int Direction();
        private Direction SelectDirection(int destinationFloor)
        {
            if (CurrentFloor < destinationFloor)
            {
                State = ElevatorState.GoesUp;
                return GoFloorUp;
            }
            else
            {
                State = ElevatorState.GoesDown;
                return GoFloorDown;
            }
        }

        public void GoToFloor(int destinationFloor)
        {
            if (destinationFloor < _minFloor || destinationFloor > _maxFloor)
            {
                throw new ArgumentOutOfRangeException(nameof(destinationFloor), destinationFloor,
                    $"Указан не верный этаж. Диапазон допустимых этажей: {_minFloor}-{_maxFloor}.");
            }
            if (CurrentFloor != destinationFloor)
            {
                Direction move = SelectDirection(destinationFloor);
                string direction = State == ElevatorState.GoesUp ? "вверх" : "вниз";
                Console.WriteLine($"Лифт {CabinNumber}. Едет {direction}.");

                while (CurrentFloor != destinationFloor)
                {
                    Console.WriteLine($"Лифт {CabinNumber}. На {CurrentFloor} этаже.");
                    Thread.Sleep(1000 * SecondsToGoOneFloor);
                    move.Invoke();
                }
            }
            Console.WriteLine($"Лифт {CabinNumber}. На {CurrentFloor} этаже.");
            Console.WriteLine($"Лифт {CabinNumber}. Двери открываются.");
            State = ElevatorState.OpensTheDoors;
        }

        public void PressFloorButton(int destinationFloor)
        {
            if (destinationFloor < _minFloor || destinationFloor > _maxFloor)
                throw new ArgumentOutOfRangeException(
                    nameof(destinationFloor),
                    destinationFloor,
                    $"Указан не верный этаж. Диапазон допустимых этажей: {_minFloor}-{_maxFloor}.");

            if (CurrentFloor != destinationFloor)
            {
                Console.WriteLine($"Лифт {CabinNumber}. Выбран {destinationFloor} этаж.");
                Console.WriteLine($"Лифт {CabinNumber}. Двери закрываются.");
                State = ElevatorState.ClosesTheDoors;

                GoToFloor(destinationFloor);

                Console.WriteLine($"Лифт {CabinNumber}. Двери закрываются.");
                State = ElevatorState.ClosesTheDoors;
            }

        }

        public void PressCloseDoorsButton()
        {
            Console.WriteLine($"Лифт {CabinNumber}. Двери закрываются.");
            State = ElevatorState.ClosesTheDoors;
        }

        public void PressOpenDoorsButton()
        {
            Console.WriteLine($"Лифт {CabinNumber}. Двери открываются.");
            State = ElevatorState.OpensTheDoors;
        }

        public void PressCallDispatcherButton()
        {
            Console.WriteLine($"Лифт {CabinNumber}. Вызов диспетчера.");
        }

        public void DetectMovementBetweenDoors()
        {
            Console.WriteLine($"Лифт {CabinNumber}. Зафиксировано движение между дверьми.");
        }


        public void DetectNoMovementBetweenDoors()
        {
            Console.WriteLine($"Лифт {CabinNumber}. Нет движения между дверьми.");
        }

        public void AddObserver(IObserver o)
        {
            _observers.Add(o);
        }

        public void RemovdeObserver(IObserver o)
        {
            _observers.Remove(o);
        }

        public void NotifyObservers()
        {
            foreach (IObserver o in _observers)
            {
                o.Update(this);
            }
        }
    }
}
