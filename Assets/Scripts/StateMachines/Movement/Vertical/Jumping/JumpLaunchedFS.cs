using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpLaunchedFS : JumpFS {
        private float timeLapsed;

        public JumpLaunchedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig, float timeLapsed)
            : base(behaviour, jump, jumpConfig) {
            this.timeLapsed = timeLapsed;
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || OutOfJumps()) return;
            Mathf.Clamp(Config.jumpsLeft--, 0, Config.maxJumps);
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }

        public override void Enter() => Rig.gravityScale = Config.lowJumpMultiplier;

        public override void Update() {
            if (Rig.velocity.y < 0 || timeLapsed >= Config.jumpDuration) Jump.RaiseChangeStateEvent(JumpStates.Falling);

            timeLapsed += Time.deltaTime;
        }
        public override float Force() => Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity;
    }
}