﻿using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class LockedFS : JumpFS {
        public LockedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig) :
            base(behaviour, jump, jumpConfig) { }

        public override void AcceptJumpInput(InputAction.CallbackContext context) { }
        public override void AcceptLockJumpInput(object sender) => Jump.RaiseChangeStateEvent(JumpStates.Grounded);
        public override void AcceptUnlockJumpInput(object sender) => Jump.RaiseChangeStateEvent(JumpStates.Grounded);
    }
}