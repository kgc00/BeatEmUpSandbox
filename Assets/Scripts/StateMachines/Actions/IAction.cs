using StateMachines.Messages;
using UnityEngine.InputSystem;

namespace StateMachines.Actions {
    public interface IAction {
        InputEventData EventData { get; }
    }
}