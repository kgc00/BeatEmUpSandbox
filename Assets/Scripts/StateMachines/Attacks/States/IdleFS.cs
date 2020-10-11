﻿using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.Observer;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class IdleFS : AttackFS {
        private GameObject punch1;

        public IdleFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit, UnitMovementData movementDataValues) : base(
            behaviour, stateMachine, kit, movementDataValues) { }

        public override void Enter() => InputLockObserver.UnlockMovementInput(behaviour);
        public override void Exit() { }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (IsJumpState() || IsDashState()) return;
            InputLockObserver.LockMovementInput(behaviour);

            IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralOne);
        }

        public override void AcceptMoveInput(InputAction.CallbackContext context) { }
    }
}