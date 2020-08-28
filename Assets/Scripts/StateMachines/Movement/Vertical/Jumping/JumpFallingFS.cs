using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpFallingFS : JumpFS{
        public JumpFallingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump, jumpConfig) { }
        public override void AcceptJumpInput(InputAction.CallbackContext context) { }

        public override void Update() {    
            if (Rig.velocity.y < 0) Rig.gravityScale = Config.fallMultiplier;
            else Rig.gravityScale = Config.lowJumpMultiplier;
        }

        public override void OnCollisionEnter2D(Collision2D other) {
            base.OnCollisionEnter2D(other);

            if (!other.gameObject.CompareTag("Board")) return;

            Jump.ChangeState(new JumpGroundedFS(Behaviour, Jump, Config));
            Rig.drag = Config.groundedLinearDrag;
        }
    }
}