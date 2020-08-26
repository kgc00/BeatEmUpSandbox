using StateMachines.Actions;

namespace StateMachines.Messages {
    public interface IActionProperty {
        IAction Action { get; }
    }
}