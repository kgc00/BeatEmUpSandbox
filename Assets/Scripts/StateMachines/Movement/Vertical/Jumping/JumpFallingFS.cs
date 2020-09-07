using StateMachines.Movement.Models;
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

        public override void OnCollisionEnter2D_RPC() {
            base.OnCollisionEnter2D_RPC();

            Jump.RaiseChangeStateEvent(JumpStates.Grounded);
            Rig.drag = Config.groundedLinearDrag;
        }
    }
}