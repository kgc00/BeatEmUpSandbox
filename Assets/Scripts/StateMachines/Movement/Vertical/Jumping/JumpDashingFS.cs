using System;
using Photon.Pun;
using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpDashingFS : JumpFS {
        private float timeLapsed;
        private float dashDir;
        private static readonly int AirDash = Animator.StringToHash("AirDash");

        public JumpDashingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig)
            : base(behaviour, jump, jumpConfig) { }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || OutOfJumps()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }

        public override void AcceptMoveInput(InputAction.CallbackContext context) {
            Jump.Values.moveDir = context.ReadValue<Single>();
        }

        public override void Enter() {
            Mathf.Clamp(Config.dashesLeft--, 0, Config.maxDashes);
            Rig.gravityScale = 0f;
            RemoveYVelocity();
            dashDir = Jump.Values.moveDir == 0 ? Behaviour.transform.localScale.x : Jump.Values.moveDir;
            Animator.SetTrigger(AirDash);
        }

        public override void Exit() {
            RemoveXVelocity();
        }

        public override void Update() {
            timeLapsed += Time.deltaTime;

            if (timeLapsed < Config.dashDuration) return;

            Jump.RaiseChangeStateEvent(JumpStates.Falling);
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.dashHorizontalVelocity, Config.maxDashVelocity, dashDir,
                    Rig.velocity.x), 0);
    }
}