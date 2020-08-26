using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpLaunchingFS : JumpFS {
        private float timeLapsed;
        public JumpLaunchingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump, jumpConfig) { }
        public override void Update() {
            timeLapsed += Time.deltaTime;

            if (timeLapsed < Config.jumpDuration) return;
            
            Jump.ChangeState(new JumpLaunchedFS(Behaviour, Jump, Config, timeLapsed));
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Canceled) return;

            Jump.ChangeState(new JumpLaunchedFS(Behaviour, Jump, Config, timeLapsed));
        }

        public override void Enter() {
            Animator.SetTrigger(Jumping);
            Rig.gravityScale = 1f;
            Rig.drag = Config.aerialLinearDrag;
        }

        public override float Force() => Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity;
    }
}