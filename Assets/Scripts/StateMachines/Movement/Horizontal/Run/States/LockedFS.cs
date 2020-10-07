using System;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run.States {
    internal class LockedFS : RunFS {
        public LockedFS(GameObject behaviour, RunConfig config, RunFSM runFsm) : base(behaviour, config,
            runFsm) { }

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            StateMachine.RaiseSetMoveDirEvent(context.ReadValue<Single>(), Behaviour.transform.localScale, ViewId);
        }

        protected override void _OnCollisionEnter2D_RPC() { }

        protected override Vector2 _Force() => Vector2.zero;

        protected override void _AcceptLockInput() { }

        protected override void _AcceptUnlockInput() {
            var moving = Math.Abs(StateMachine.UnitState.moveDir) > .01f;

            StateMachine.RaiseChangeRunStateEvent(moving ? RunStates.Moving : RunStates.Idle, ViewId);
        }
    }
}