using UnityEngine.InputSystem;

namespace StateMachines.Jumping.Interfaces {
    public interface IAcceptJumpInput {
        void AcceptJumpInput(InputAction.CallbackContext context);
    }
}