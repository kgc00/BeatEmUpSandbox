using System;
using Photon.Pun;
using StateMachines.Movement.Models;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpLaunchingFS : JumpFS {
        public JumpLaunchingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(
            behaviour, jump,
            jumpConfig) { }

        public override void Update() {
            Jump.Values.jumpTimeLapsed += Time.deltaTime;

            // checking for punismine on update seems to disturb flow of app
            if (Jump.Values.jumpTimeLapsed < Config.jumpDuration) return;

            if (!PUNIsMine) return;
            
            Jump.RaiseChangeStateEvent(JumpStates.Launched);
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Canceled) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launched);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (OutOfDashes()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Dashing);
        }

        public override void Enter() {
            Jump.Values.jumpTimeLapsed = 0;
            Jump.Values.jumpsLeft = Mathf.Clamp(Jump.Values.jumpsLeft - 1, 0, Config.maxDashes);
            Animator.SetTrigger(Jump.Values.jumpsLeft == 1 ? Jumping : DoubleJumping);
            InputLockObserver.LockRunInput(Behaviour);
            RemoveYVelocity();
            Rig.gravityScale = 1f;
            Rig.drag = Config.aerialLinearDrag;
            if (Jump.Values.moveDir != 0) Behaviour.transform.localScale = new Vector3((int) Jump.Values.moveDir, 1, 1);
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.horizontalVelocity,
                    Config.maxVelocity, Jump.Values.moveDir, Rig.velocity.x),
                Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity);
    }
}