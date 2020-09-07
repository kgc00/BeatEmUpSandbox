using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpGroundedFS : JumpFS {
        public JumpGroundedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump, jumpConfig) { }
        public override void OnCollisionEnter2D_RPC() { }
        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }
        public override void Enter() {
            Rig.gravityScale = 1f;    
            Rig.drag = Config.groundedLinearDrag;
        }

        public override void AcceptLockJumpInput() => Jump.RaiseChangeStateEvent(JumpStates.Locked);
    }
}