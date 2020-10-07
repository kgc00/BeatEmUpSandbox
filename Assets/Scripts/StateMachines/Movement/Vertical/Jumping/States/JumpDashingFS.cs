using System;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping.States {
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
            Jump.RaiseSetMoveDirEvent(context.ReadValue<Single>(), Behaviour.transform.localScale, ViewID);

        public override void Enter() {
            Jump.UnitState.dashesLeft = Mathf.Clamp(Jump.UnitState.dashesLeft - 1, 0, Config.maxDashes);
            Rig.gravityScale = 0f;
            RemoveYVelocity();
            dashDir = Jump.UnitState.moveDir == 0 ? Behaviour.transform.localScale.x : Jump.UnitState.moveDir;
            Animator.SetTrigger(AirDash);
        }

        public override void Exit() {
            RemoveXVelocity();
            Jump.UnitState.dashTimeLapsed = 0;
        }

        public override void Update() {
            Jump.UnitState.dashTimeLapsed += Time.deltaTime;

            if (Jump.UnitState.dashTimeLapsed < Config.dashDuration && !Jump.UnitState.touchingWall) return;

            Jump.RaiseChangeStateEvent(JumpStates.Falling);
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.dashHorizontalVelocity,
                    Config.maxDashVelocity, dashDir, Rig.velocity.x), 0);
    }
}