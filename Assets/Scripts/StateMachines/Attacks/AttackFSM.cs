using System;
using StateMachines.Attacks.Models;
using StateMachines.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class AttackFSM : MonoBehaviour, IAcceptAttackInput, IChangeState<AttackFS>, IHandleAttackAnimationEnter,
        IHandleAttackAnimationExit, IHandleComboChaining, IEnableAttackBuffer, IToggleHitboxes {
        public AttackFS State { get; private set; }
        [SerializeField] private AttackKit kit;

        private void Awake() {
            State = new IdleFS(gameObject, this, kit);
        }

        public void ChangeState(AttackFS newState) {
            State.Exit();
            State = newState;
            State.Enter();
        }

        public void AcceptAttackInput(InputAction.CallbackContext context) => State.AcceptAttackInput(context);

        public void HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            State.HandleAttackAnimationEnter(animator, stateInfo, layerIndex);

        public void HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            State.HandleAttackAnimationExit(animator, stateInfo, layerIndex);

        public void EnableChaining() => State.EnableChaining();
        public void EnableAttackBuffer() => State.EnableAttackBuffer();
        public void EnableHitbox() => State.EnableHitbox();
        public void DisableHitbox() => State.DisableHitbox();

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(0, 81, 410, 80));    
            GUILayout.Box("attack: " + State.GetType());
            GUILayout.EndArea();
        }
    }
}