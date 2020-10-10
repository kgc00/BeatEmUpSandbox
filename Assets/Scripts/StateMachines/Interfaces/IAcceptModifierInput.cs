using UnityEngine.InputSystem;

namespace StateMachines.Interfaces {
    public interface IAcceptModifierInput {
        void AcceptModifierInput(InputAction.CallbackContext context);
    }
}