using StateMachines.Messages;

namespace StateMachines.Actions {
    public class BaseAction : IAction {
        public InputEventData EventData { get; }

        public BaseAction(InputEventData data) {
            EventData = data;
        }
        
    }
}