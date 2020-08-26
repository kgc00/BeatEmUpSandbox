using StateMachines.Actions;

namespace StateMachines.Messages {
    public class AttackActionOccuredMessage : ActionOccuredMessage {
        public AttackActionOccuredMessage(UnitFSM unit, IAction action) {
            Action = action;
            UnitFsm = unit;
        }
    }
}