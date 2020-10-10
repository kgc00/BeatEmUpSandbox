using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping.States {
    public class JumpFallingFS : JumpFS {
        public JumpFallingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour,
            jump,
            jumpConfig) { }

        public override void Enter() {
            if (Jump.UnitState.moveDir != 0) Behaviour.transform.localScale = new Vector3((int) Jump.UnitState.moveDir, 1, 1);
            Animator.Play("player_fall");
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || OutOfJumps()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (OutOfDashes()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Dashing);
        }

        public override void Update() {
            Rig.gravityScale = Rig.velocity.y < 0
                ? Config.fallMultiplier
                : Config.lowJumpMultiplier;
        }

        public override void OnCollisionEnter2D_RPC() {
            if (!Jump.UnitState.touchingGround) return;

            Rig.drag = Config.groundedLinearDrag;

            Jump.RaiseChangeStateEvent(JumpStates.Grounded);
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.horizontalVelocity, Config.maxVelocity, Jump.UnitState.moveDir,
                    Rig.velocity.x), 0);
    }
}