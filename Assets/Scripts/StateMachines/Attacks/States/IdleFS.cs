using Photon.Pun;
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

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (IsDashState()) return;
            InputLockObserver.LockMovementInput(behaviour);

            if (IsJumpState()) IdentifyAndTransitionToAerialAttackState(AttackStates.AerialNeutralOne, .15f);
            else IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralOne);
        }

        public override void AcceptMoveInput(InputAction.CallbackContext context) { }
    }
}