using StateMachines.Movement.Models;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping.States {
    public class JumpGroundedFS : JumpFS {
        public JumpGroundedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) : base(behaviour, jump, jumpConfig) { }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }
        
        public override void Enter() {
            Rig.gravityScale = 1f;
            Rig.drag = Config.groundedLinearDrag;
            ResetMoveValues();
            InputLockObserver.UnlockRunInput(Behaviour);
        }

        public override void Update() {
            if (AnimatorStateFalling()) Animator.Play("player_idle");
        }

        private void ResetMoveValues() {
            Jump.UnitMovementData.jumpsLeft = Config.maxJumps;
            Jump.UnitMovementData.dashesLeft = Config.maxDashes;
            Jump.UnitMovementData.jumpTimeLapsed = 0;
            Jump.UnitMovementData.dashTimeLapsed = 0;
        }

        // handled by run state
        public override void AcceptMoveInput(InputAction.CallbackContext context) { }

        public override void AcceptLockJumpInput(object sender) => Jump.RaiseChangeStateEvent(JumpStates.Locked);
    }
}