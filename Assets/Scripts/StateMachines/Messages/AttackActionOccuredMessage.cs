using StateMachines.Actions;
using UnityEngine;

namespace StateMachines.Messages {
    public class AttackActionOccuredMessage : ActionOccuredMessage {
        public AttackActionOccuredMessage(GameObject behaviour, IAction action) {
            Behaviour = behaviour;
            Action = action;
        }
    }
}