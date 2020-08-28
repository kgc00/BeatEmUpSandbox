using UnityEngine.InputSystem;

namespace StateMachines.Interfaces {
    public interface IAcceptAttackInput {
        void AcceptAttackInput(InputAction.CallbackContext context);
    }
}