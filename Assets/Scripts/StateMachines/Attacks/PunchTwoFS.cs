using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class PunchTwoFS : AttackFS {
        private GameObject punch2;
        private readonly int attack2 = Animator.StringToHash("Attack2");

        public PunchTwoFS(GameObject behaviour, AttackFSM stateMachine) : base(behaviour, stateMachine) { }

        public override void Enter() {
            animator.SetTrigger(attack2);
        }

        public override void Exit() {
            animator.ResetTrigger(attack2);
        }

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;
            stateMachine.ChangeState(new PunchThreeFS(behaviour, stateMachine));
        }

        protected override void _HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
            if (IsExitingAttackState()){
                stateMachine.ChangeState(new IdleFS(behaviour, stateMachine));
            }
        }
    }
}