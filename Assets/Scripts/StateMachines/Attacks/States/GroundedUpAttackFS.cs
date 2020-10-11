using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedUpAttackFS : AttackFS {
        public GroundedUpAttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitMovementData movementDataValues) : base(behaviour, stateMachine, kit, movementDataValues) {
            hitbox = HitboxFromKit(GetType()); }

        public override void Enter() {
            animator.Play("ground-up-attack");
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ground-up-attack"))
                animator.Play("ground-up-attack");
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (chainingEnabled) IdentifyAndTransitionToGroundedMovementOrAttackState( true);
        }
        
        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }

        public override void HandleExitAnimation() {
            HandleStateChange(AttackStates.Idle);
        }
    }
}