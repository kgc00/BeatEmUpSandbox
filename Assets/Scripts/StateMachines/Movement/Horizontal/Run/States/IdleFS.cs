using System;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run.States {
    public class IdleFS : RunFS {
        public IdleFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm)
            : base(behaviour, runConfig, runFsm) { }

        public override void Enter() => Animator.Play("player_idle");

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (IsJumpState()) return;

            StateMachine.RaiseChangeRunStateEvent(RunStates.Dash, ViewId);
        }

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            var lookDir = context.ReadValue<Single>() == 0
                ? Behaviour.transform.localScale.x
                : StateMachine.UnitMovementData.moveDir;
            
            StateMachine.RaiseSetMoveDirEvent(context.ReadValue<Single>(), new Vector3(lookDir, 1, 1), ViewId);
            
            var moving = Math.Abs(StateMachine.UnitMovementData.moveDir) > .01f;

            if (moving) StateMachine.RaiseChangeRunStateEvent(RunStates.Moving, ViewId);
        }
        
        protected override void _OnCollisionEnter2D_RPC() {
            // UpdateAnimations();
        }

        protected override Vector2 _Force() => Vector2.zero;

        protected override void _AcceptUnlockInput() { }
    }
}