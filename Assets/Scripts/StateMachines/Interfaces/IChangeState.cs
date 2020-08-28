using StateMachines.Movement.Horizontal.Run;

namespace StateMachines.Interfaces {
    public interface IChangeState<T> where T : FSMState<T> {
        void ChangeState(T newState);
    }
}