namespace StateMachines {
    public abstract class FSMState<T> where T : FSMState<T> {
        public virtual void Enter(){}
        public virtual FSMState<T> Update() => null;
        public virtual FSMState<T> LateUpdate() => null;
        public virtual FSMState<T> FixedUpdate() => null;
        public virtual void Exit(){}
    }
}