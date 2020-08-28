namespace StateMachines {
    public abstract class FSMState<T> where T : FSMState<T> {
        public virtual void Enter(){}
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit(){}
    }
}