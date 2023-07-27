using ElevatorControlSystem.Floor;

namespace ElevatorControlSystem.Elevator
{
    public interface IObservable
    {
        void AddObserver(IObserver o);
        void RemovdeObserver(IObserver o);
        void NotifyObservers();
    }
}
