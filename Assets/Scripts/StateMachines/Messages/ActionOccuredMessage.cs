using StateMachines.Actions;
using UnityEngine;

namespace StateMachines.Messages {
    public abstract class ActionOccuredMessage : IMessage, IActionProperty, IBehaviourProperty {
        public IAction Action { get; set; }
        public GameObject Behaviour { get; set; }
    }
}