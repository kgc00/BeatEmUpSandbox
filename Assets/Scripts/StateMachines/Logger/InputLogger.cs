using System.Collections.Generic;
using StateMachines.Actions;
using UnityEngine;

namespace StateMachines.Logger {
    public class InputLogger {
        public List<IAction> Actions { get; private set; }

        public void AddAction(IAction action) {
            Actions.Add(action);
        }
    }
}