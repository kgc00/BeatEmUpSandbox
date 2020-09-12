using UnityEngine.InputSystem;

namespace StateMachines.Interfaces {
    public interface IAcceptDashInput {
        void AcceptDashInput(InputAction.CallbackContext context);
    }
}