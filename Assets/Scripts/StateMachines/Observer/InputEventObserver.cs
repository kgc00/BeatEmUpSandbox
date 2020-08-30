using System;
using System.Collections.Generic;
using StateMachines.Messages;

namespace StateMachines.Observer {
    public class InputEventObserver {
        public List<InputEvent> events;
        public Action<List<InputEvent>> OnInputEvent = delegate {  };
        public InputEventObserver() {
            events = new List<InputEvent>();
        }
        
        public void AddEvent(InputEvent e) {
            events.Add(e);
            OnInputEvent(events);
        }
    }
}