using ElevatorControlSystem.Elevator;

namespace ElevatorControlSystem.Floor
{
    public interface IObserver
    {
        void Update(IObservable subject);
    }
}
