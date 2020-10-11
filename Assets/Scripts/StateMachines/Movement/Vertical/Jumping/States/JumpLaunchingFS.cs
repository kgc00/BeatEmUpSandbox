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
            Jump.UnitMovementData.jumpTimeLapsed += Time.deltaTime;
            
            if (Jump.UnitMovementData.jumpTimeLapsed < Config.jumpDuration) return;
            
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
            Jump.UnitMovementData.jumpTimeLapsed = 0;
            Jump.UnitMovementData.jumpsLeft = Mathf.Clamp(Jump.UnitMovementData.jumpsLeft - 1, 0, Config.maxDashes);
            HandleAnimation();
            InputLockObserver.LockRunInput(Behaviour);
            RemoveYVelocity();
            Rig.gravityScale = 1f;
            Rig.drag = Config.aerialLinearDrag;
            if (Jump.UnitMovementData.moveDir != 0) Behaviour.transform.localScale = new Vector3((int) Jump.UnitMovementData.moveDir, 1, 1);
        }

        private void HandleAnimation() {
            Animator.Play(Jump.UnitMovementData.jumpsLeft == 1 ? "player_jump" : "player_double_jump");
        }

        public override Vector2 Force() =>
            new Vector2(
                ProvideCappedHorizontalForce(Config.horizontalVelocity,
                    Config.maxVelocity, Jump.UnitMovementData.moveDir, Rig.velocity.x),
                Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity);
    }
}