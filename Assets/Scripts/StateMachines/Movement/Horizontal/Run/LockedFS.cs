using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    internal class LockedFS : RunFS {
        public LockedFS(GameObject behaviour, RunConfig config, RunFSM runFsm, float dir = 0f) : base(behaviour, config,
            runFsm, dir) { }

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Started) return;

            MoveDir = context.ReadValue<Single>();
        }

        protected override void _OnCollisionEnter2D(Collision2D other) { }

        protected override float _Force() => 0;


        protected override void _AcceptLockInput() { }

        protected override void _AcceptUnlockInput() {
            var moving = Math.Abs(MoveDir) > .01f;
            
            if (moving) StateMachine.ChangeState(new MovingFS(Behaviour, Config, StateMachine, MoveDir));
            else StateMachine.ChangeState(new IdleFS(Behaviour, Config, StateMachine, MoveDir));
        }
    }
}