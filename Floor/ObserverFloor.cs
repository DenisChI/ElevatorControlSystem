using ElevatorControlSystem.Elevator;

namespace ElevatorControlSystem.Floor
{
    public class ObserverFloor : IObserver
    {
        private readonly int _floorNumber;
        public int FirstCabinCurrentFloor { get; set; }

        public ElevatorState FirstCabinCurrentStatus { get; set; }

        public int SecondCabinCurrentFloor { get; set; }

        public ElevatorState SecondCabinCurrentStatus { get; set; }

        public CallButtonState CallButtonStatus { get; set; }

        public ObserverFloor(int floorNumber)
        {
            _floorNumber = floorNumber;
            CallButtonStatus = CallButtonState.NotCalled;
        }

        public void PressCallButton()
        {
            Console.WriteLine($"Этаж {_floorNumber}. Вызван лифт.");
            CallButtonStatus = CallButtonState.Called;
        }

        public void Update(IObservable subject)
        {
            if (subject is ObservableElevator elevator)
            {
                switch (elevator.CabinNumber)
                {
                    case 1:
                        {
                            FirstCabinCurrentFloor = elevator.CurrentFloor;
                            FirstCabinCurrentStatus = elevator.State;
                            break;
                        }
                    case 2:
                        {
                            SecondCabinCurrentFloor = elevator.CurrentFloor;
                            SecondCabinCurrentStatus = elevator.State;
                            break;
                        }
                }
            }
        }
    }
}
