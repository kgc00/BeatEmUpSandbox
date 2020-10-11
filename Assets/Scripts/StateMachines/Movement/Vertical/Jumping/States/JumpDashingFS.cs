using System;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping.States {
    public class JumpDashingFS : JumpFS {
        private static readonly int AirDash = Animator.StringToHash("Air Dash");
        private float dashDir;

        public JumpDashingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig)
            : base(behaviour, jump, jumpConfig) { }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || OutOfJumps()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }

        public override void AcceptMoveInput(InputAction.CallbackContext context) =>
            Jump.RaiseSetMoveDirEvent(context.ReadValue<Single>(), Behaviour.transform.localScale, ViewID);

        public override void Enter() {
            Jump.UnitMovementData.dashesLeft = Mathf.Clamp(Jump.UnitMovementData.dashesLeft - 1, 0, Config.maxDashes);
            Rig.gravityScale = 0f;
            RemoveYVelocity();
            dashDir = Jump.UnitMovementData.moveDir == 0 ? Behaviour.transform.localScale.x : Jump.UnitMovementData.moveDir;
            Animator.Play(AirDash);
        }

        public override void Exit() {
            RemoveXVelocity();
            Jump.UnitMovementData.dashTimeLapsed = 0;
        }

        public override void Update() {
            Jump.UnitMovementData.dashTimeLapsed += Time.deltaTime;

            if (Jump.UnitMovementData.dashTimeLapsed < Config.dashDuration && !Jump.UnitMovementData.touchingWall) return;

            Jump.RaiseChangeStateEvent(JumpStates.Falling);
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.dashHorizontalVelocity,
                    Config.maxDashVelocity, dashDir, Rig.velocity.x), 0);
    }
}