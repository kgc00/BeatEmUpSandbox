using System;
using Photon.Pun;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpDashingFS : JumpFS {
        private static readonly int AirDash = Animator.StringToHash("AirDash");
        private float dashDir;

        public JumpDashingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig)
            : base(behaviour, jump, jumpConfig) { }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || OutOfJumps()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }

        public override void AcceptMoveInput(InputAction.CallbackContext context) =>
            Jump.RaiseSetMoveDirEvent(context.ReadValue<Single>(), ViewID);

        public override void Enter() {
            Jump.Values.dashesLeft = Mathf.Clamp(Jump.Values.dashesLeft - 1, 0, Config.maxDashes);
            Rig.gravityScale = 0f;
            RemoveYVelocity();
            dashDir = Jump.Values.moveDir == 0 ? Behaviour.transform.localScale.x : Jump.Values.moveDir;
            Animator.SetTrigger(AirDash);
        }

        public override void Exit() {
            RemoveXVelocity();
            Jump.Values.dashTimeLapsed = 0;
        }

        public override void Update() {
            Jump.Values.dashTimeLapsed += Time.deltaTime;

            if (Jump.Values.dashTimeLapsed < Config.dashDuration) return;

            if (!PUNIsMine) return;
            
            Jump.RaiseChangeStateEvent(JumpStates.Falling);
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.dashHorizontalVelocity,
                    Config.maxDashVelocity, dashDir, Rig.velocity.x), 0);
    }
}