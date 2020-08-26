using UnityEngine.InputSystem;

namespace StateMachines.Movement.Interfaces {
    public interface IAcceptJumpInput {
        void AcceptJumpInput(InputAction.CallbackContext context);
    }
}