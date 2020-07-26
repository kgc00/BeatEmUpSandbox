namespace StateMachines {
    public abstract class FSMState {
        public virtual void Enter(){}
        public virtual FSMState Update() => null;
        public virtual void Exit(){}
    }
}