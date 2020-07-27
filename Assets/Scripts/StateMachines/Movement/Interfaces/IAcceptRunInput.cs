using UnityEngine.InputSystem;

namespace StateMachines.Movement.Interfaces {
    public interface IAcceptRunInput {
        void AcceptMoveInput(InputAction.CallbackContext context);
    }
}