using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpFallingFS : JumpFS {
        public JumpFallingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig, float moveDir) : base(behaviour,
            jump,
            jumpConfig, moveDir) { }

        public override void Enter() {
            if (MoveDir != 0) Behaviour.transform.localScale = new Vector3((int) MoveDir, 1, 1);
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || OutOfJumps()) return;
            Mathf.Clamp(Config.jumpsLeft--, 0, Config.maxJumps);
            Jump.RaiseChangeStateEvent(JumpStates.Launching, MoveDir);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (OutOfDashes()) return;
            Mathf.Clamp(Config.dashesLeft--, 0, Config.maxDashes);
            Jump.RaiseChangeStateEvent(JumpStates.Dashing, MoveDir);
        }

        public override void Update() {
            if (Rig.velocity.y < 0) Rig.gravityScale = Config.fallMultiplier;
            else Rig.gravityScale = Config.lowJumpMultiplier;
        }

        public override void OnCollisionEnter2D_RPC() {
            base.OnCollisionEnter2D_RPC();

            Jump.RaiseChangeStateEvent(JumpStates.Grounded, MoveDir);
            Rig.drag = Config.groundedLinearDrag;
        }

        public override Vector2 Force() =>
            new Vector2(ProvideCappedHorizontalForce(Config.horizontalVelocity,Config.maxVelocity, MoveDir, Rig.velocity.x), 0);
    }
}