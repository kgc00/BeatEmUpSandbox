using StateMachines.Actions;

namespace StateMachines.Messages {
    public abstract class ActionOccuredMessage : IMessage, IActionProperty, IUnitProperty {
        public UnitFSM UnitFsm { get; set; }
        public IAction Action { get; set; }
    }
}