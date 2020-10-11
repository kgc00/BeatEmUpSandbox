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
        IAcceptAttackInput, IAcceptDashInput, IAcceptJumpInput,
        IAcceptModifierInput {
        public const float BufferDuration = 0.15f;
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

        public void AcceptModifierInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                (context.phase != InputActionPhase.Performed && context.phase != InputActionPhase.Canceled)) return;
            Actions.Insert(0, new ModifyAction(new InputEventData(context, Time.time)));
        }

        private void LateUpdate() {
            for (int i = Actions.Count - 1; i >= 0; i--) {
                if (CalcDifference(i) > EventTimeDeletionThreshold) Actions.RemoveAt(i);
            }
        }

        private float CalcDifference(int i) => Helpers.GenerateTimeStamp() - Actions[i].EventData.Timestamp;

        public bool IsForwardAttack() {
            /* only true if:
                - most recent action is attack 
                - prev action is forward press
            */
            if (Actions.Count < 2) return false;
            var didAttack = Actions[0].EventData.ActionName == ActionNames.Attack &&
                            Actions[0].EventData.Phase == InputActionPhase.Performed;
            var didPressForward = Actions[1].EventData.ActionName == ActionNames.Modify &&
                                  Actions[1].EventData.Phase == InputActionPhase.Performed &&
                                  (Vector2) Actions[1].EventData.Value ==
                                  new Vector2(gameObject.transform.localScale.x, 0);

            var performedQuickly = CalcDifference(1) < BufferDuration;

            var isForwardAttack = didAttack && didPressForward && performedQuickly;
            if (isForwardAttack) ClearPerformedActions();

            return isForwardAttack;
        }

        private void ClearPerformedActions() {
            Actions.RemoveRange(0, 2);
        }

        private void OnGUI() {
            if (!photonView.IsMine) return;

            GUILayout.Box("Input Event Count: " + Actions.Count);
        }

        public bool IsUpAttack() {
            if (Actions.Count < 2) return false;
            var didAttack = Actions[0].EventData.ActionName == ActionNames.Attack &&
                            Actions[0].EventData.Phase == InputActionPhase.Performed;
            var didPressUp = Actions[1].EventData.ActionName == ActionNames.Modify &&
                             Actions[1].EventData.Phase == InputActionPhase.Performed &&
                             (Vector2) Actions[1].EventData.Value == new Vector2(0, 1);
            var performedQuickly = CalcDifference(1) < BufferDuration;

            var isUpAttack = didAttack && didPressUp && performedQuickly;

            if (isUpAttack) ClearPerformedActions();

            return isUpAttack;
        }

        public bool IsRecentInput(float bufferLength = BufferDuration, int i = 0) =>
            Actions.Count > i - 1
            && Actions.Count > 0
            && CalcDifference(i) < bufferLength;


        public bool IsInputOfType(string actionName) => Actions.Count != 0
                                                        && (Actions[0].EventData.Phase == InputActionPhase.Performed
                                                            // case where player lets go of attack and cancelled is recorded as most recent input
                                                            || Actions[0].EventData.Phase == InputActionPhase.Canceled)
                                                        && Actions[0].EventData.ActionName == actionName;

        public bool QueryReleasedInputOfType(string actionName) {
            if (IsPerformedInputOfType(actionName, 0)) return false;

            for (int i = 0; i < Actions.Count; i++) {
                if (IsRecentInput(BufferDuration, i) && IsReleasedInputOfType(actionName, i))
                    return true;
            }

            return false;
        }

        private bool IsPerformedInputOfType(string actionName, int i) =>
            Actions.Count > i - 1 &&
            Actions[i].EventData.Phase == InputActionPhase.Performed
            && Actions[i].EventData.ActionName == actionName;
        
        private bool IsReleasedInputOfType(string actionName, int i) =>
            Actions.Count > i - 1 &&
            Actions[i].EventData.Phase == InputActionPhase.Canceled
            && Actions[i].EventData.ActionName == actionName;

        public bool DidBufferAttackInput(float bufferLength = BufferDuration) =>
            IsRecentInput(bufferLength)
            && IsInputOfType(ActionNames.Attack);

        public bool DidBufferReleasedJumpInput(float bufferLength = BufferDuration) =>
            IsRecentInput(bufferLength)
            && IsInputOfType(ActionNames.Jump);

        public bool DidBufferJumpInput(float bufferLength = BufferDuration) =>
            IsRecentInput(bufferLength)
            && IsInputOfType(ActionNames.Jump);

        public bool DidBufferMoveInput(float bufferLength = BufferDuration) =>
            IsRecentInput(bufferLength)
            && IsInputOfType(ActionNames.Move);

        public bool DidBufferDashInput(float bufferLength = BufferDuration) =>
            IsRecentInput(bufferLength)
            && IsInputOfType(ActionNames.Dash);
    }
}