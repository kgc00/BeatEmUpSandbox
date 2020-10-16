using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class AerialNeutralTwo : AttackFS {
        private readonly int aerial2 = Animator.StringToHash("air-neutral-2");

        public AerialNeutralTwo(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) :
            base(behaviour, stateMachine, kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType());            
            isAerialState = true;

        }

        public override void Enter() {
            animator.Play(aerial2);
            EnterAerialAttackState();
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("air-neutral-2"))
                animator.Play(aerial2);
        }


        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToAerialAttackState(null, .05f);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (chainingEnabled) IdentifyAndTransitionToAerialAttackState(null, .05f);
        }
    }
}