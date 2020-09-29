using System;

namespace StateMachines.Observer {
    public static class InputLockObserver {
        public static Action<System.Object> LockMovementInput = delegate { };
        public static Action<System.Object> UnlockMovementInput = delegate { };
        public static void OnLockMovementInput(object sender) => LockMovementInput(sender);
        public static void OnUnlockMovementInput(object sender) => UnlockMovementInput(sender);

        public static Action<System.Object> LockJumpInput = delegate { };
        public static Action<System.Object> UnlockJumpInput = delegate { };
        public static void OnLockJumpInput(object sender) => LockJumpInput(sender);
        public static void OnUnlockJumpInput(object sender) => UnlockJumpInput(sender);
        
        
        public static Action<System.Object> LockRunInput = delegate { };
        public static Action<System.Object> UnlockRunInput = delegate { };
        public static void OnLockRunInput(object sender) => LockRunInput(sender);
        public static void OnUnlockRunInput(object sender) => UnlockRunInput(sender);
        
        
        public static Action LockAttackInput = delegate { };
        public static Action UnlockAttackInput = delegate { };
        public static void OnLockAttackInput() => LockAttackInput();
        public static void OnUnlockAttackInput() => UnlockAttackInput();
    }
    
    // public static class AttackActionObserver {
    //     public static event EventHandler<AttackActionOccuredMessage> AttackActionOccured = delegate { };
    //
    //     public static void OnAttackAction(AttackActionOccuredMessage action) {
    //         if (AttackActionOccured != null) {
    //             EventHandler<AttackActionOccuredMessage> handler = AttackActionOccured;
    //             handler(null, action);
    //         }
    //     }
    // }
}