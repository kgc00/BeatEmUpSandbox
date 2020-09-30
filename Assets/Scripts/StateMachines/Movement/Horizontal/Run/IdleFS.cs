using System;
using StateMachines.Movement.Models;
using StateMachines.Network;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public class IdleFS : RunFS {
        public IdleFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm)
            : base(behaviour, runConfig, runFsm) { }

        public override void Enter() => UpdateAnimations();

        public override void Exit() => Animator.ResetTrigger(Idle);

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (IsJumpState()) return;

            StateMachine.RaiseChangeRunStateEvent(RunStates.Dash, ViewId);
        }

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            StateMachine.Values.moveDir = context.ReadValue<Single>();
            var moving = Math.Abs(StateMachine.Values.moveDir) > .01f;

            if (moving) StateMachine.RaiseChangeRunStateEvent(RunStates.Moving, ViewId);
        }

        protected override void UpdateAnimations() {
            if (!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) Animator.SetTrigger(Idle);
        }

        protected override void _OnCollisionEnter2D_RPC() {
            UpdateAnimations();
        }

        protected override Vector2 _Force() => Vector2.zero;

        protected override void _AcceptUnlockInput() { }
    }
}