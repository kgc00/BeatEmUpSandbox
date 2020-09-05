namespace StateMachines.Interfaces {
    public interface IHandleLockedJumpInput {
        void AcceptLockJumpInput();
        void AcceptUnlockJumpInput();
    }
}