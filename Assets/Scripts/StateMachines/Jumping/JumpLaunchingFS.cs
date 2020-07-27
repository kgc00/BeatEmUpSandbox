using StateMachines.Jumping.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Jumping {
    public class JumpLaunchingFS : JumpFS {
        private float timeLapsed;
        public JumpLaunchingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump, jumpConfig) { }
        public override FSMState Update() {
            timeLapsed += Time.deltaTime;

            if (timeLapsed < Config.jumpDuration) return null;
            
            Jump.ChangeState(new JumpLaunchedFS(Behaviour, Jump, Config, timeLapsed));
            return null;
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Canceled) return;

            Jump.ChangeState(new JumpLaunchedFS(Behaviour, Jump, Config, timeLapsed));
        }

        public override void Enter() {
            Animator.SetTrigger(Jumping);
            Rig.gravityScale = 1f;            
            Rig.drag = 0f;
        }

        public override float Force() => Mathf.Abs(Rig.velocity.y) >= 3.25f ? 0 : Config.jumpVelocity;
    }
}