using System;
using StateMachines.Messages;
using UnityEngine;

namespace StateMachines.Observer {
    public static class AttackActionObserver {
        public static Action<AttackActionOccuredMessage> AttackActionOccured = delegate { };
        public static void OnAttackAction(AttackActionOccuredMessage action) => AttackActionOccured(action);
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