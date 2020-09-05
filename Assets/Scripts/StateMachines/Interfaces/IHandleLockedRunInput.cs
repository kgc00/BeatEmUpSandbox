namespace StateMachines.Interfaces {
    public interface IHandleLockedRunInput {
        void AcceptLockRunInput();
        void AcceptUnlockRunInput();
    }
}