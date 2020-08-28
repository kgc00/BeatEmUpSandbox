using UnityEngine.InputSystem;

namespace StateMachines.Interfaces {
    public interface IAcceptRunInput {
        void AcceptMoveInput(InputAction.CallbackContext context);
    }
}