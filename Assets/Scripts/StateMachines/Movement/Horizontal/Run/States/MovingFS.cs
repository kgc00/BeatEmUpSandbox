using System;
using Photon.Pun;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using Common;

namespace StateMachines.Movement.Horizontal.Run.States {
    public class MovingFS : RunFS {
        private bool isMine;

        public MovingFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm)
            : base(behaviour, runConfig, runFsm) {
            isMine = Behaviour.GetPhotonView().IsMine;
        }

        public override void Enter() => UpdateAnimations();
        public override void Exit() => Animator.ResetTrigger(Running);

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            var lookDir = context.ReadValue<Single>() == 0
                ? Behaviour.transform.localScale.x
                : StateMachine.UnitState.moveDir;

            StateMachine.RaiseSetMoveDirEvent(context.ReadValue<Single>(), new Vector3(lookDir, 1, 1), ViewId);

            var moving = Math.Abs(StateMachine.UnitState.moveDir) > .01f;

            if (!moving) StateMachine.RaiseChangeRunStateEvent(RunStates.Idle, ViewId);
        }

        protected override void UpdateAnimations() {
            Transform.localScale = StateMachine.UnitState.moveDir > 0 ? Vector3.one : new Vector3(-1, 1, 1);

            if (!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Run")) Animator.SetTrigger(Running);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (IsJumpState()) return;
            StateMachine.RaiseChangeRunStateEvent(RunStates.Dash, ViewId);
        }

        private static int CappedMoveVelocity() => 0;
        private float NormalMoveVelocity() => StateMachine.UnitState.moveDir * Config.runVelocity;
        private bool HitSpeedCap(float rigX) => Mathf.Abs(rigX) >= Config.maxVelocity;

        protected override void _OnCollisionEnter2D_RPC() {
            UpdateAnimations();
        }

        public override void Update() {
            HandleAnimations();
            ExitIfIdle();
        }

        private void ExitIfIdle() {
            if (StateMachine.UnitState.moveDir == 0 && isMine) {
                StateMachine.RaiseChangeRunStateEvent(RunStates.Idle, ViewId);
            }
        }

        private void HandleAnimations() {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsTag("Run")) return;

            UpdateAnimations();
        }

        protected override Vector2 _Force() {
            var rigX = Rig.velocity.x;

            var xVel = HitSpeedCap(rigX) && SandboxUtils.IsForwardMovement(StateMachine.UnitState.moveDir, rigX)
                ? CappedMoveVelocity()
                : NormalMoveVelocity();
            return new Vector2(xVel, 0);
        }

        protected override void _AcceptUnlockInput() { }
    }
}