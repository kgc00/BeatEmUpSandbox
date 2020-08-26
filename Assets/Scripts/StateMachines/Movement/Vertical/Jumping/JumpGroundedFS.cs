using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpGroundedFS : JumpFS {
        public JumpGroundedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump, jumpConfig) { }
        public override void OnCollisionEnter2D(Collision2D other) { }
        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;

            Jump.ChangeState(new JumpLaunchingFS(Behaviour, Jump, Config));
        }
        public override void Enter() {
            Rig.gravityScale = 1f;    
            Rig.drag = Config.groundedLinearDrag;
        }
    }
}