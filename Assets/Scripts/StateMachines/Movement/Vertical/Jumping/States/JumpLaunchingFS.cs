using StateMachines.Movement.Models;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping.States {
    public class JumpLaunchingFS : JumpFS {
        public JumpLaunchingFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(
            behaviour, jump,
            jumpConfig) { }

        public override void Update() {
            Jump.UnitState.jumpTimeLapsed += Time.deltaTime;
            
            if (!AnimatorStateJumping() && !AnimatorStateDoubleJumping())
                HandleAnimation();

            if (Jump.UnitState.jumpTimeLapsed < Config.jumpDuration) return;
            
            Jump.RaiseChangeStateEvent(JumpStates.Launched);
        }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Canceled) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launched);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (OutOfDashes()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Dashing);
        }

        public override void Enter() {
            Jump.UnitState.jumpTimeLapsed = 0;
            Jump.UnitState.jumpsLeft = Mathf.Clamp(Jump.UnitState.jumpsLeft - 1, 0, Config.maxDashes);
            HandleAnimation();
            InputLockObserver.LockRunInput(Behaviour);
            RemoveYVelocity();
            Rig.gravityScale = 1f;
            Rig.drag = Config.aerialLinearDrag;
            if (Jump.UnitState.moveDir != 0) Behaviour.transform.localScale = new Vector3((int) Jump.UnitState.moveDir, 1, 1);
        }

        private void HandleAnimation() {
            Animator.Play(Jump.UnitState.jumpsLeft == 1 ? "player_jump" : "player_double_jump");
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.horizontalVelocity,
                    Config.maxVelocity, Jump.UnitState.moveDir, Rig.velocity.x),
                Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity);
    }
}