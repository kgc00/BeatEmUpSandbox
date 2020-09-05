using System;
using System.Collections.Generic;
using StateMachines.Attacks.Models;
using StateMachines.Observer;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class PunchOneFS : AttackFS {
        private GameObject punch1;
        private readonly int attack1 = Animator.StringToHash("Attack1");
        private bool chainingEnabled;
        private bool bufferEnabled;
        private Queue<InputAction.CallbackContext> bufferedActions = new Queue<InputAction.CallbackContext>();

        private readonly GameObject hitbox;

        public PunchOneFS(GameObject behaviour, AttackFSM stateMachine, AttackKit kit) : base(behaviour, stateMachine,
            kit) {
            hitbox = HitboxFromKit(GetType());
        }


        public override void Enter() {
            animator.SetTrigger(attack1);
        }

        public override void Exit() {
            animator.ResetTrigger(attack1);
        }

        protected override void _EnableChaining() {
            chainingEnabled = true;
            if (bufferedActions.Count <= 0) return;
            
            // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html#responding-to-actions

            // Note: The contents of the structure are only valid for the duration of the callback.
            // In particular, it isn't safe to store the received context and later access its properties from outside the callback.

            // var context = bufferedActions.Dequeue();
            // _AcceptAttackInput(context);
            stateMachine.ChangeState(new PunchTwoFS(behaviour, stateMachine, kit));
        }

        protected override void _EnableAttackBuffer() => bufferEnabled = true;

        protected override void _AcceptAttackInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;

            if (chainingEnabled) {
                stateMachine.ChangeState(new PunchTwoFS(behaviour, stateMachine, kit));
                return;
            }

            if (bufferEnabled) bufferedActions.Enqueue(context);
        }

        protected override void _HandleAttackAnimationEnter(
            Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        protected override void _HandleAttackAnimationExit(
            Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) {
            if (IsExitingAttackState())
                stateMachine.ChangeState(new IdleFS(behaviour, stateMachine, kit));
        }

        protected override void _EnableHitbox() => hitbox.SetActive(true);
        protected override void _DisableHitbox() => hitbox.SetActive(false);

        private void LogInfo() {
            Debug.Log(animator.GetNextAnimatorClipInfo(0));
            Debug.Log(animator.GetNextAnimatorClipInfo(0)[0].clip);
            Debug.Log(animator.GetNextAnimatorClipInfo(0)[0].clip.name);

            Debug.Log("CURRENT STATE INFO --- ");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
            Debug.Log("Logging Attack1");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack1"));
            Debug.Log("Logging Attack2");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack2"));
            Debug.Log("Logging Idle");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
            Debug.Log("Logging Jump");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"));
            Debug.Log("Logging Run");
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"));

            Debug.Log("NEXT STATE INFO --- ");
            Debug.Log(animator.GetNextAnimatorStateInfo(0));
            Debug.Log("Logging Attack1");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack1"));
            Debug.Log("Logging Attack2");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Attack2"));
            Debug.Log("Logging Idle");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Idle"));
            Debug.Log("Logging Jump");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Jump"));
            Debug.Log("Logging Run");
            Debug.Log(animator.GetNextAnimatorStateInfo(0).IsTag("Run"));
        }
    }
}