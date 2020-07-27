using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Jumping {
    public class JumpFallingFS : JumpFS{
        public JumpFallingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump, jumpConfig) { }
        public override void AcceptJumpInput(InputAction.CallbackContext context) { }

        public override FSMState Update() {
            if (Rig.velocity.y < 0) Rig.gravityScale = Config.fallMultiplier;
            else Rig.gravityScale = Config.lowJumpMultiplier;
            return null;
        }

        public override void OnCollisionEnter2D(Collision2D other) {
            base.OnCollisionEnter2D(other);
            Jump.ChangeState(new JumpGroundedFS(Behaviour, Jump, Config));
            Rig.drag = 20f;
        }
    }
}