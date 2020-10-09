using System;
using System.Collections.Generic;
using General;
using Photon.Pun;
using StateMachines.Actions;
using StateMachines.Interfaces;
using StateMachines.Messages;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Logger {
    public class InputLogger : MonoBehaviourPun, IAcceptRunInput,
        IAcceptAttackInput, IAcceptDashInput, IAcceptJumpInput {
        public const float EventTimeDeletionThreshold = 0.5f; // delete events older than this value
        public List<IAction> Actions { get; private set; } = new List<IAction>();

        public void AcceptMoveInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Insert(0, new MoveAction(new InputEventData(context, Time.time)));
        }

        public void AcceptAttackInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Insert(0, new AttackAction(new InputEventData(context, Time.time)));
        }

        public void AcceptDashInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Insert(0, new DashAction(new InputEventData(context, Time.time)));
        }

        public void AcceptJumpInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Insert(0, new JumpAction(new InputEventData(context, Time.time)));
        }

        private void LateUpdate() {
            for (int i = Actions.Count - 1; i >= 0; i--) {
                if (CalcDifference(i) > EventTimeDeletionThreshold) Actions.RemoveAt(i);
            }
        }

        private float CalcDifference(int i) {
            return Helpers.GenerateTimeStamp() - Actions[i].EventData.Timestamp;
        }


        private void OnGUI() {
            if (!photonView.IsMine) return;

            GUILayout.Box("Input Event Count: " + Actions.Count);
        }
    }
}