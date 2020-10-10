using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedUpAttackFS : AttackFS {
        public GroundedUpAttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitState stateValues) : base(behaviour, stateMachine, kit, stateValues) {
            hitbox = HitboxFromKit(GetType()); }

        public override void Enter() {
            Debug.Log("Entered grounded up");
            animator.Play("ground-up-attack");
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(Animator animator1, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void HandleExitAnimation() {
            HandleStateChange(AttackStates.Idle);
        }
    }
}