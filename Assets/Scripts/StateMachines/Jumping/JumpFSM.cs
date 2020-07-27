using System;
using StateMachines.Jumping.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Jumping {
    public class JumpFSM : IProvideForce, IAcceptCollisionEnter, IAcceptJumpInput {
        private JumpFS state;

        public JumpFSM(GameObject behaviour, JumpConfig jumpConfig) {
            state = new JumpGroundedFS(behaviour, this, jumpConfig);
        }

        public float Force() => state.Force();

        public void ChangeState(JumpFS newState) {
            state.Exit();
            state = newState;
            state.Enter();
        }

        public void Update() => state.Update();

        public void OnCollisionEnter2D(Collision2D other) => state.OnCollisionEnter2D(other);
        public void AcceptJumpInput(InputAction.CallbackContext context) => state.AcceptJumpInput(context);
    }
}