using StateMachines.Actions;

namespace StateMachines.Messages {
    public class InputEvent : IActionProperty, ITimestampProperty {
        // TODO: state - pressed | released
        public IAction Action { get; }
        public float Timestamp { get; }

        public InputEvent(IAction action, float timestamp) {
            Action = action;
            Timestamp = timestamp;
        }
    }
}