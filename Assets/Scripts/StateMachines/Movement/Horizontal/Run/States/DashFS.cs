using System;
using Photon.Pun;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run.States {
    public class DashFS : RunFS {
        private float timeLapsed;
        private static readonly int Dash = Animator.StringToHash("Dash");
        private float dashDir;

        public DashFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm) : base(behaviour, runConfig,
            runFsm) { }

        public override void Enter() {
            dashDir = StateMachine.UnitState.moveDir == 0 ? Behaviour.transform.localScale.x : StateMachine.UnitState.moveDir;
            Animator.SetTrigger(Dash);
        }

        // maybe only accept input at the end of dash
        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            StateMachine.RaiseSetMoveDirEvent(context.ReadValue<Single>(), Behaviour.transform.localScale, ViewId);
        }

        public override void Update() {
            timeLapsed += Time.deltaTime;

            if (timeLapsed < Config.dashDuration) return;

            var moving = Math.Abs(StateMachine.UnitState.moveDir) > .01f;

            StateMachine.RaiseChangeRunStateEvent(moving ? RunStates.Moving : RunStates.Idle, ViewId);
        }

        protected override void _OnCollisionEnter2D_RPC() { }

        private int CappedMoveVelocity() => 0;

        private bool HitSpeedCap(float rigX) => Mathf.Abs(rigX) >= Config.maxVelocity;

        // should instantly accelerate to cap
        protected override Vector2 _Force() => new Vector2(Config.dashVelocity * dashDir, 0);

        protected override void _AcceptUnlockInput() { }
    }
}