using StateMachines.Movement.Horizontal.Run;

namespace StateMachines.Interfaces {
    public interface IChangeState<in FSMState> {
        void ChangeState(FSMState newState);
    }
}