using System;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    internal class LockedFS : RunFS {
        public LockedFS(GameObject behaviour, RunConfig config, RunFSM runFsm, float dir = 0f) : base(behaviour, config,
            runFsm, dir) { }

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            MoveDir = context.ReadValue<Single>();
        }

        protected override void _OnCollisionEnter2D_RPC() { }

        protected override float _Force() {
            if(!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) Animator.SetTrigger(Idle);
            return 0;
        }


        protected override void _AcceptLockInput() { }

        protected override void _AcceptUnlockInput() {
            var moving = Math.Abs(MoveDir) > .01f;
            
            if (moving) StateMachine.RaiseChangeStateEvent(RunStates.Moving, MoveDir);
            else StateMachine.RaiseChangeStateEvent(RunStates.Idle, MoveDir);
        }
    }
}