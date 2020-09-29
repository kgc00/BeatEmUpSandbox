namespace StateMachines.Interfaces {
    public interface IHandleLockedJumpInput {
        void AcceptLockJumpInput(object sender);
        void AcceptUnlockJumpInput(object sender);
    }
}