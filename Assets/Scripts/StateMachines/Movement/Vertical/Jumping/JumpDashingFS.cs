using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpDashingFS : JumpFS {
        public JumpDashingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump, jumpConfig) { }
        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;
            Mathf.Clamp(Config.jumpsLeft--, 0, Config.maxJumps);
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }

        public override void Enter() {
            Rig.gravityScale = 0f;
        }
    }
}