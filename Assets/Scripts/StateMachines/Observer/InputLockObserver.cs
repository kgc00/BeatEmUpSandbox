using System;
using StateMachines.Interfaces;
using StateMachines.Messages;
using UnityEngine;

namespace StateMachines.Observer {
    public static class InputLockObserver {
        public static Action LockInput = delegate { };
        public static Action UnlockInput = delegate { };
        public static void OnLockInput() => LockInput();
        public static void OnUnlockInput() => UnlockInput();
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