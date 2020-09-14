using System;
using Photon.Pun;
using StateMachines.Movement.Models;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpLaunchingFS : JumpFS {
        private float timeLapsed;

        public JumpLaunchingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig, float moveDir) : base(
            behaviour, jump,
            jumpConfig, moveDir) { }

        public override void Update() {
            timeLapsed += Time.deltaTime;

            // if (timeLapsed <= 0.2f) {
            //     if (Config.jumpsLeft == 0 && !AnimatorStateJumping()) Animator.SetTrigger(DoubleJumping);
            //     else if (Config.jumpsLeft == 1 && !AnimatorStateJumping()) Animator.SetTrigger(Jumping);
            // }
            
            if (timeLapsed < Config.jumpDuration) return;

            Jump.RaiseChangeStateEvent(JumpStates.Launched, MoveDir, timeLapsed);
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Canceled) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launched, MoveDir, timeLapsed);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (OutOfDashes()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Dashing, MoveDir);
        }

        public override void Enter() {
            // Animator.SetTrigger(Jumping);
            Mathf.Clamp(Config.jumpsLeft--, 0, Config.maxJumps);
            Animator.SetTrigger(Config.jumpsLeft == 1 ? Jumping : DoubleJumping);
            InputLockObserver.LockRunInput();
            RemoveYVelocity();
            Rig.gravityScale = 1f;
            Rig.drag = Config.aerialLinearDrag;
            if (MoveDir != 0) Behaviour.transform.localScale = new Vector3((int) MoveDir, 1, 1);
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.horizontalVelocity, Config.maxVelocity, MoveDir, Rig.velocity.x),
                Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity);
    }
}