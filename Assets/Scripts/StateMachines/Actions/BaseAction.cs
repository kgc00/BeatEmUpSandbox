using StateMachines.Messages;

namespace StateMachines.Actions {
    public class BaseAction : IAction {
        public InputEventData EventData { get; }

        protected BaseAction(InputEventData data) {
            EventData = data;
        }
    }
}