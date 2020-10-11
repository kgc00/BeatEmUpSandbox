using System.Collections.Generic;
using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedNeutralTwoFS : AttackFS {
        private GameObject punch2;
        private readonly int attack2 = Animator.StringToHash("GroundedNeutral2");

        public GroundedNeutralTwoFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit, UnitMovementData movementDataValues) : base(behaviour, stateMachine,
            kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.Play(attack2);
        }

        public override void Exit() { }

        protected override void _EnableHitbox() => hitbox.SetActive(true);
        protected override void _DisableHitbox() => hitbox.SetActive(false);

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralThree, true);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralThree);
        }
    }
}