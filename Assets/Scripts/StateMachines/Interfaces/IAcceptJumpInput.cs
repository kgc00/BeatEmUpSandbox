using UnityEngine.InputSystem;

namespace StateMachines.Interfaces {
    public interface IAcceptJumpInput {
        void AcceptJumpInput(InputAction.CallbackContext context);
    }
}