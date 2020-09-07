using System;
using StateMachines.Movement.Models;
using StateMachines.Network;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public class IdleFS : RunFS {
        public IdleFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm, float dir = 0f) 
            : base(behaviour, runConfig, runFsm, dir) { }

        public override void Enter() => UpdateAnimations();

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            MoveDir = context.ReadValue<Single>();
            var moving = Math.Abs(MoveDir) > .01f;

            if (moving) StateMachine.RaiseChangeStateEvent(RunStates.Moving, MoveDir);
        }

        protected override void UpdateAnimations() {
            if(!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) Animator.SetTrigger(Idle);
            
            Animator.ResetTrigger(Running);
        }

        protected override void _OnCollisionEnter2D_RPC() {
            UpdateAnimations();
        }

        protected override float _Force() {
            // helps with serializing aniamtion states across the network
            UpdateAnimations();
            return 0;
        }

        protected override void _AcceptUnlockInput() { }
    }
}