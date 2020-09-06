using StateMachines.Movement;

namespace StateMachines.Messages {
    public interface IUnitProperty {
        MovementFSM MovementFSM { get; }
    }
}