using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Horizontal.Run {
    public class MovingFS : RunFS {
        public MovingFS(GameObject behaviour, RunConfig runConfig, RunFSM runFsm, float dir) 
            : base(behaviour, runConfig, runFsm, dir) { }

        public override void Enter() => UpdateAnimations();

        protected override void _AcceptMoveInput(InputAction.CallbackContext context) {
            MoveDir = context.ReadValue<Single>();
            var moving = Math.Abs(MoveDir) > .01f;

            if (!moving) StateMachine.ChangeState(new IdleFS(Behaviour, Config, StateMachine, MoveDir));
        }

        protected override void UpdateAnimations() {
            Animator.SetTrigger(Running);
            Transform.localScale = MoveDir > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            
            if(Animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"))Animator.ResetTrigger(Running);
        }

        private static int CappedMoveVelocity() => 0;
        private float NormalMoveVelocity() => MoveDir * Config.runVelocity;
        private bool HitSpeedCap(float rigX) => Mathf.Abs(rigX) >= Config.maxVelocity;
        private bool IsForwardMovement(float rigX) => Mathf.Sign(MoveDir) == Mathf.Sign(rigX);

        protected override void _OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Board")) return;

            UpdateAnimations();
        }

        protected override float _Force() {
            var rigX = Rig.velocity.x;

            return HitSpeedCap(rigX) && IsForwardMovement(rigX) ? CappedMoveVelocity() : NormalMoveVelocity();
        }

        protected override void _AcceptUnlockInput() { }
    }
}