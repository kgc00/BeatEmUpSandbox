using NUnit.Framework;
using StateMachines.Actions;
using StateMachines.Logger;
using StateMachines.Messages;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestSandbox.EditorMode {
    public class KeyLoggerTests {
        [Test]
        public void DoesTrackInputEvents() {
            var logger = new InputLogger();
            // logger.AddAction(new BaseAction(new InputEventData(new InputAction.CallbackContext())));
        }
    }


}