﻿using System;
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
        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            var lookDir = context.ReadValue<Single>() == 0
                ? Behaviour.transform.localScale.x
                : StateMachine.UnitMovementData.moveDir;

            StateMachine.RaiseSetMoveDirEvent(context.ReadValue<Single>(), new Vector3(lookDir, 1, 1), ViewId);

            var moving = Math.Abs(StateMachine.UnitMovementData.moveDir) > .01f;

            if (!moving) StateMachine.RaiseChangeRunStateEvent(RunStates.Idle, ViewId);
        }

        protected override void _OnCollisionEnter2D_RPC() { }

        protected override void UpdateAnimations() {
            Transform.localScale = StateMachine.UnitMovementData.moveDir > 0 ? Vector3.one : new Vector3(-1, 1, 1);

            if (!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Run")) Animator.Play("player_run");
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (IsJumpState()) return;
            StateMachine.RaiseChangeRunStateEvent(RunStates.Dash, ViewId);
        }

        private static int CappedMoveVelocity() => 0;
        private float NormalMoveVelocity() => StateMachine.UnitMovementData.moveDir * Config.runVelocity;
        private bool HitSpeedCap(float rigX) => Mathf.Abs(rigX) >= Config.maxVelocity;

        public override void Update() {
            if (!Animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"))
                Animator.Play("player_run");
            ExitIfIdle();
        }

        private void ExitIfIdle() {
            if (StateMachine.UnitMovementData.moveDir == 0 && isMine) {
                StateMachine.RaiseChangeRunStateEvent(RunStates.Idle, ViewId);
            }
        }

        protected override Vector2 _Force() {
            var rigX = Rig.velocity.x;

            var xVel = HitSpeedCap(rigX) && SandboxUtils.IsForwardMovement(StateMachine.UnitMovementData.moveDir, rigX)
                ? CappedMoveVelocity()
                : NormalMoveVelocity();
            return new Vector2(xVel, 0);
        }

        protected override void _AcceptUnlockInput() { }
    }
}