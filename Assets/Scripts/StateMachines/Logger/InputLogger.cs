using System;
using System.Collections.Generic;
using Photon.Pun;
using StateMachines.Actions;
using StateMachines.Interfaces;
using StateMachines.Messages;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Logger {
    public class InputLogger : MonoBehaviourPun, IAcceptRunInput, 
        IAcceptAttackInput, IAcceptDashInput, IAcceptJumpInput {
        public List<IAction> Actions { get; private set; } = new List<IAction>();

        public void AcceptMoveInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Add(new MoveAction(new InputEventData(context, Time.time)) as IAction);
        }

        public void AcceptAttackInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Add(new AttackAction(new InputEventData(context, Time.time)) as IAction);
        }

        public void AcceptDashInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Add(new DashAction(new InputEventData(context, Time.time)) as IAction);
        }

        public void AcceptJumpInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Add(new JumpAction(new InputEventData(context, Time.time)) as IAction);
        }
    }
}