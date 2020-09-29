namespace StateMachines.Interfaces {
    public interface IHandleLockedRunInput {
        void AcceptLockRunInput(object sender);
        void AcceptUnlockRunInput(object sender);
    }
}