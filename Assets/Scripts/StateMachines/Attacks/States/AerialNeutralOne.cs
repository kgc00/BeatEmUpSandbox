using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class AerialNeutralOne : AttackFS {
        private readonly int aerial1 = Animator.StringToHash("air-neutral-1");

        public AerialNeutralOne(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) :
            base(behaviour, stateMachine, kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.Play(aerial1);
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("air-neutral-1"))
                animator.Play(aerial1);
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralTwo, true);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) IdentifyAndTransitionToGroundedAttackState(AttackStates.GroundedNeutralTwo);
        }
    }
}