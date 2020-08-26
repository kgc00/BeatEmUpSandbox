using System;
using StateMachines.Actions;
using StateMachines.Messages;
using StateMachines.Observer;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;


namespace StateMachines.KeyLogger
{

 
    public class KeyLogger : MonoBehaviour {
        private InputEventObserver inputEventObserver;

        private void Start() {
            inputEventObserver = GetComponentInParent<UnitFSM>().inputEventObserver;
        }

        public void AcceptAttackInput(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;

            print("accept!");
            inputEventObserver.AddEvent(new InputEvent(new AttackAction(), Time.time));
        }
    }
}