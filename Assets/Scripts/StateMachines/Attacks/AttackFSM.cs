using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Attacks.Models;
using StateMachines.Attacks.States;
using StateMachines.Interfaces;
using StateMachines.Network;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    [RequireComponent(typeof(UnitDataStore))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class AttackFSM : MonoBehaviourPun, IAcceptAttackInput,
        IChangeStatePun<AttackStates>, IHandleAttackAnimationEnter,
        IHandleAttackAnimationExit, IHandleComboChaining,
        IEnableAttackBuffer, IToggleHitboxes,
        IAcceptJumpInput, IAcceptRunInput,
    IHandleExitAnimationEvents{
        public AttackFS State { get; private set; }
        [SerializeField] private UnitDataStore dataStore;
        public UnitMovementData UnitMovementData { get; private set; }
        [SerializeField] public AttackKit kit;

        private void Start() {
            UnitMovementData = dataStore.store;
            State = new IdleFS(gameObject, this, kit, UnitMovementData);
            State.Enter();
        }

        public void RaiseChangeStateEvent(AttackStates newState) =>
            photonView.RPC("ChangeState", RpcTarget.All, newState);

        [PunRPC]
        public void ChangeState(AttackStates newState) {
            State.Exit();
            State = StateFactory.AttackFSFromEnum(newState, this);
            State.Enter();
        }

        public void AcceptAttackInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine) return;
            State.AcceptAttackInput(context);
        }

        public void HandleAttackAnimationEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            State.HandleAttackAnimationEnter(animator, stateInfo, layerIndex);

        public void HandleAttackAnimationExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            State.HandleAttackAnimationExit(animator, stateInfo, layerIndex);

        public void EnableChaining() => photonView.RPC("EnableChaining_RPC", RpcTarget.All);

        [PunRPC]
        void EnableChaining_RPC() => State.EnableChaining();

        public void EnableAttackBuffer() => photonView.RPC("EnableAttackBuffer_RPC", RpcTarget.All);

        [PunRPC]
        void EnableAttackBuffer_RPC() => State.EnableAttackBuffer();

        public void EnableHitbox() => photonView.RPC("EnableHitbox_RPC", RpcTarget.All);

        [PunRPC]
        void EnableHitbox_RPC() => State.EnableHitbox();

        public void DisableHitbox() => photonView.RPC("DisableHitbox_RPC", RpcTarget.All);

        [PunRPC]
        void DisableHitbox_RPC() => State.DisableHitbox();


        // private void OnGUI() {
        //     if (!photonView.IsMine) return;
        //     
        //     GUILayout.BeginArea(new Rect(0, 141, 410, 80));
        //     GUILayout.Box("attack: " + State.GetType());
        //     GUILayout.EndArea();
        // }

        public void AttackConnected(HitBox hitBox, Collider2D other) {
            if (!other.gameObject.CompareTag("Enemy")) return;

            State.AttackConnected(hitBox, other);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                context.phase != InputActionPhase.Performed &&
                context.phase != InputActionPhase.Canceled) return;
            State.AcceptMoveInput(context);
        }

        public void AcceptJumpInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                context.phase != InputActionPhase.Performed &&
                context.phase != InputActionPhase.Canceled) return;
            State.AcceptJumpInput(context);
        }

        // may need to check for photonview ismine
        public void HandleExitAnimation() => State.HandleExitAnimation();
    }
}