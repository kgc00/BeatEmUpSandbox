using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpLaunchingFS : JumpFS {
        private float timeLapsed;

        public JumpLaunchingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump,
            jumpConfig) { }

        public override void Update() {
            timeLapsed += Time.deltaTime;

            if (timeLapsed < Config.jumpDuration) return;

            Jump.RaiseChangeStateEvent(JumpStates.Launched, timeLapsed);
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Canceled) return;

            Jump.RaiseChangeStateEvent(JumpStates.Launched, timeLapsed);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if(OutOfDashes()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Dashing);
        }

        public override void Enter() {
            Animator.SetTrigger(Config.jumpsLeft == 1 ? Jumping: DoubleJumping);
            RemoveVelocity();
            Rig.gravityScale = 1f;
            Rig.drag = Config.aerialLinearDrag;
        }

        private void RemoveVelocity() {
            /* remove downward velocity for case of
             *  doing a double jump while falling at a great speed.
             *  if we didn't do this, the jump would not raise the player up
             * (all the negative y velocity would eat the movement)
            */
            
            var vel = Rig.velocity;
            vel.y = 0;

            Rig.velocity = vel;
        }

        public override float Force() => Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity;
    }
}