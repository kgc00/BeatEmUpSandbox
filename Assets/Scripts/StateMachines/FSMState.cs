namespace StateMachines {
    public abstract class FSMState {
        public virtual void Enter(){}
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit(){}
    }
}