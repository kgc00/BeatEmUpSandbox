using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class AerialForwardAttack : AttackFS {
        private readonly int aerialForwardAttack = Animator.StringToHash("air-forward-attack");

        public AerialForwardAttack(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) :
            base(behaviour, stateMachine, kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());
        }

        public override void Enter() {
            animator.Play(aerialForwardAttack);
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("air-forward-attack"))
                animator.Play(aerialForwardAttack);
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