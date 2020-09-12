using System;
using System.Collections;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public class DashFS : RunFS {
        private float timeLapsed;
        private static readonly int Dash = Animator.StringToHash("Dash");
        private float dashDir;

        public DashFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm, float dir) : base(behaviour, runConfig,
            runFsm, dir) { }

        public override void Enter() {
            dashDir = MoveDir == 0 ? Behaviour.transform.localScale.x : MoveDir;
            Animator.SetTrigger(Dash);
        }

        // maybe only accept input at the end of dash
        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            MoveDir = context.ReadValue<Single>();
        }

        public override void Update() {
            timeLapsed += Time.deltaTime;

            if (timeLapsed < Config.dashDuration) return;

            var moving = Math.Abs(MoveDir) > .01f;

            StateMachine.RaiseChangeStateEvent(moving ? RunStates.Moving : RunStates.Idle, MoveDir);
        }

        protected override void _OnCollisionEnter2D_RPC() { }

        private int CappedMoveVelocity() => 0;

        private bool HitSpeedCap(float rigX) => Mathf.Abs(rigX) >= Config.maxVelocity;

        // should instantly accelerate to cap
        protected override float _Force() => Config.dashVelocity * dashDir;
        // HitSpeedCap(Rig.velocity.x) ? CappedMoveVelocity() : Config.maxVelocity;

        protected override void _AcceptUnlockInput() { }
    }
}