using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class LockedFS : JumpFS {
        public LockedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig, float moveDir) :
            base(behaviour, jump, jumpConfig, moveDir) { }

        public override void AcceptJumpInput(InputAction.CallbackContext context) { }
        public override void AcceptLockJumpInput() => Jump.RaiseChangeStateEvent(JumpStates.Grounded, MoveDir);
        public override void AcceptUnlockJumpInput() => Jump.RaiseChangeStateEvent(JumpStates.Grounded, MoveDir);
    }
}