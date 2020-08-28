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
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") ||
                animator.GetCurrentAnimatorStateInfo(0).IsTag("Run") ||
                animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump") ){
                // Debug.Log("CURRENT STATE INFO --- ");
                // Debug.Log(animator.GetCurrentAnimatorClipInfo(0));
                // Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip);
                // Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                // Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
                // Debug.Log("Logging Attack1");
                // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack1"));
                // Debug.Log("Logging Attack2");
                // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack2"));
                // Debug.Log("Logging Idle");
                // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
                // Debug.Log("Logging Jump");
                // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"));
                // Debug.Log("Logging Run");
                // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"));
                //
                // Debug.Log("NEXT STATE INFO --- ");
                // Debug.Log(animator.GetNextAnimatorClipInfo(0));
                // Debug.Log(animator.GetNextAnimatorClipInfo(0)[0].clip);
                // Debug.Log(animator.GetNextAnimatorClipInfo(0)[0].clip.name);
                // Debug.Log(animator.GetNextAnimatorStateInfo(0));
                // Debug.Log("Logging Attack1");
                // Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack1"));
                // Debug.Log("Logging Attack2");
                // Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack2"));
                // Debug.Log("Logging Idle");
                // Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Idle"));
                // Debug.Log("Logging Jump");
                // Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Jump"));
                // Debug.Log("Logging Run");
                // Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Run"));
                stateMachine.ChangeState(new IdleFS(behaviour, stateMachine));
            }
        }
    }
}