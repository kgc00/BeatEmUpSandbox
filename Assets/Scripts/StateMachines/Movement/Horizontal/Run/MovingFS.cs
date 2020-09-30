using System;
using Photon.Pun;
using StateMachines.Movement.Models;
using StateMachines.Network;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public class MovingFS : RunFS {
        private bool isMine;

        public MovingFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm)
            : base(behaviour, runConfig, runFsm) {
            isMine = Behaviour.GetPhotonView().IsMine;
        }

        public override void Enter() => UpdateAnimations();
        public override void Exit() => Animator.ResetTrigger(Running);

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            StateMachine.Values.moveDir = context.ReadValue<Single>();
            var moving = Math.Abs(StateMachine.Values.moveDir) > .01f;

            if (!moving) StateMachine.RaiseChangeRunStateEvent(RunStates.Idle, ViewId);
        }

        protected override void UpdateAnimations() {
            Transform.localScale = StateMachine.Values.moveDir > 0 ? Vector3.one : new Vector3(-1, 1, 1);

            if (!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Run")) Animator.SetTrigger(Running);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (IsJumpState()) return;
            StateMachine.RaiseChangeRunStateEvent(RunStates.Dash, ViewId);
        }

        private static int CappedMoveVelocity() => 0;
        private float NormalMoveVelocity() => StateMachine.Values.moveDir * Config.runVelocity;
        private bool HitSpeedCap(float rigX) => Mathf.Abs(rigX) >= Config.maxVelocity;
        private bool IsForwardMovement(float rigX) => Mathf.Sign(StateMachine.Values.moveDir) == Mathf.Sign(rigX);

        protected override void _OnCollisionEnter2D_RPC() {
            UpdateAnimations();
        }

        public override void Update() {
            HandleAnimations();
            ExitIfIdle();
        }

        private void ExitIfIdle() {
            if (StateMachine.Values.moveDir == 0 && isMine) {
                StateMachine.RaiseChangeRunStateEvent(RunStates.Idle, ViewId);
            }
        }

        private void HandleAnimations() {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsTag("Run")) return;

            UpdateAnimations();
        }

        protected override Vector2 _Force() {
            var rigX = Rig.velocity.x;

            var xVel = HitSpeedCap(rigX) && IsForwardMovement(rigX) ? CappedMoveVelocity() : NormalMoveVelocity();
            return new Vector2(xVel, 0);
        }

        protected override void _AcceptUnlockInput() { }
    }
}