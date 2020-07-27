using StateMachines.Jumping.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Jumping {
    public class JumpLaunchedFS : JumpFS {
        private float timeLapsed;

        public JumpLaunchedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig, float timeLapsed)
            : base(behaviour, jump, jumpConfig) {
            this.timeLapsed = timeLapsed;
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) { }

        public override void Enter() => Rig.gravityScale = Config.lowJumpMultiplier;

        public override void Update() {
            if (Rig.velocity.y < 0 || timeLapsed >= Config.jumpDuration) Jump.ChangeState(new JumpFallingFS(Behaviour, Jump, Config));

            timeLapsed += Time.deltaTime;
        }
        public override float Force() => Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity;
    }
}