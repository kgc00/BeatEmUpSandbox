using System;
using StateMachines.Interfaces;
using StateMachines.Messages;
using UnityEngine;

namespace StateMachines.Observer {
    public static class InputLockObserver {
        public static Action LockMovementInput = delegate { };
        public static Action UnlockMovementInput = delegate { };
        public static void OnLockMovementInput() => LockMovementInput();
        public static void OnUnlockMovementInput() => UnlockMovementInput();

        public static Action LockJumpInput = delegate { };
        public static Action UnlockJumpInput = delegate { };
        public static void OnLockJumpInput() => LockJumpInput();
        public static void OnUnlockJumpInput() => UnlockJumpInput();
        
        
        public static Action LockRunInput = delegate { };
        public static Action UnlockRunInput = delegate { };
        public static void OnLockRunInput() => LockRunInput();
        public static void OnUnlockRunInput() => UnlockRunInput();
        
        
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