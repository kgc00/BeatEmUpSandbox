using Photon.Pun;
using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedNeutralThreeFS : AttackFS {
        private GameObject punch3;
        private readonly int attack3 = Animator.StringToHash("GroundedNeutral3");

        public GroundedNeutralThreeFS(GameObject behaviour, AttackFSM stateMachine, AttackKit attackKit,
            UnitMovementData movementDataValues) : base(behaviour,
            stateMachine, attackKit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.Play(attack3);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(null);
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(null, true);
        }
    }
}