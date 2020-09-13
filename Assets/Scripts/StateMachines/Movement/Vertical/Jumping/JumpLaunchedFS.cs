 using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpLaunchedFS : JumpFS {
        private float timeLapsed;

        public JumpLaunchedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig, float moveDir,
            float timeLapsed)
            : base(behaviour, jump, jumpConfig, moveDir) {
            this.timeLapsed = timeLapsed;
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || OutOfJumps()) return;
            Mathf.Clamp(Config.jumpsLeft--, 0, Config.maxJumps);
            Jump.RaiseChangeStateEvent(JumpStates.Launching, MoveDir);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (OutOfDashes()) return;
            Mathf.Clamp(Config.dashesLeft--, 0, Config.maxDashes);            
            Debug.Log("AcceptDashInput in Launched- MoveDir = " + MoveDir);
            Jump.RaiseChangeStateEvent(JumpStates.Dashing, MoveDir);
        }

        public override void Enter() => Rig.gravityScale = Config.lowJumpMultiplier;

        public override void Update() {
            if (Rig.velocity.y < 0 || timeLapsed >= Config.jumpDuration)
                Jump.RaiseChangeStateEvent(JumpStates.Falling, MoveDir);

            timeLapsed += Time.deltaTime;
        }

        public override Vector2 Force() => new Vector2(
            ProvideCappedHorizontalForce(Config.horizontalVelocity, Config.maxVelocity,MoveDir, Rig.velocity.x),
            Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity);
    }
}