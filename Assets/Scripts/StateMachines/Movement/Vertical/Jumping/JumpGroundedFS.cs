using Photon.Pun;
using StateMachines.Movement.Models;
using StateMachines.Observer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpGroundedFS : JumpFS {
        public JumpGroundedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig, float moveDir = 0f) : base(behaviour, jump, jumpConfig, moveDir) { }
        public override void OnCollisionEnter2D_RPC() { }
        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launching, MoveDir);
        }
        
        public override void Enter() {
            Rig.gravityScale = 1f;
            Rig.drag = Config.groundedLinearDrag;
            Config.jumpsLeft = Config.maxJumps;
            Config.dashesLeft = Config.maxDashes;
            InputLockObserver.UnlockRunInput();
        }

        public override void AcceptLockJumpInput() => Jump.RaiseChangeStateEvent(JumpStates.Locked, MoveDir);
    }
}