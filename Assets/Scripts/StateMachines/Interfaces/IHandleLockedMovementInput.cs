namespace StateMachines.Interfaces {
    public interface IHandleLockedMovementInput {
        void AcceptLockMovementInput(object sender);
        void AcceptUnlockMovementInput(object sender);
    }
}