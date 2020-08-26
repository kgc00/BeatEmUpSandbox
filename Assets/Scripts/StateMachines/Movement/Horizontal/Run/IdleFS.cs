using System;
using StateMachines.Movement.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public class IdleFS : RunFS {
        public IdleFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm, float dir = 0f) 
            : base(behaviour, runConfig, runFsm, dir) { }

        public override void Enter() {
            base.Enter();
            UpdateAnimations();
        }

        public override void Exit() {
            base.Exit();
            UpdateAnimations();
        }

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Started) return;

            MoveDir = context.ReadValue<Single>();
            var moving = Math.Abs(MoveDir) > .01f;

            if (moving) StateMachine.ChangeState(new MovingFS(Behaviour, Config, StateMachine, MoveDir));
        }

        private void UpdateAnimations() {
            Animator.ResetTrigger(Running);
            Animator.SetTrigger(Idle);
        }

        protected override void _OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Board")) return;

            UpdateAnimations();
        }

        protected override float _Force() => 0;
    }
}