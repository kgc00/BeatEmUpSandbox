using StateMachines.Attacks.Models;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks.States {
    public class GroundedForwardAttackFS : AttackFS {
        public GroundedForwardAttackFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit,
            UnitState stateValues) : base(behaviour, stateMachine, kit, stateValues) {
            hitbox = HitboxFromKit(GetType()); 
        }

        public override void Enter() {
            animator.Play("ground-forward-attack");
        }

        public override void Update() {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ground-forward-attack"))
                animator.Play("ground-forward-attack");
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) { }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(Animator animator1, AnimatorStateInfo stateInfo,
            int layerIndex) { }
    }
}