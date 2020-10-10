using System;
using StateMachines.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Messages {
    public class InputEventData {
        public readonly InputActionPhase Phase;
        public readonly string ActionName;
        public readonly float Timestamp;
        public readonly object Value;

        public InputEventData(InputAction.CallbackContext input, float timestamp) {
            Phase = input.phase;
            ActionName = input.action.name;
            Timestamp = timestamp;
            
            if (ActionName == "Modify Action") {
                Value = input.ReadValue<Vector2>();
            } else {
                Value = input.ReadValue<Single>();
            }
        }
    }
}