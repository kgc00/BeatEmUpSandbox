using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Attacks.Models;
using StateMachines.Attacks.States;
using StateMachines.Interfaces;
using StateMachines.Network;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Attacks {
    public class AttackFSM : MonoBehaviourPun, IAcceptAttackInput,
        IChangeStatePun<AttackStates>,
        IHandleAttackAnimationEnter,
        IHandleAttackAnimationExit, IHandleComboChaining, IEnableAttackBuffer, IToggleHitboxes {
        public AttackFS State { get; private set; }
        [SerializeField] public AttackKit kit;

        private void Awake() {
            State = new IdleFS(gameObject, this, kit);
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

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(0, 81, 410, 80));
            GUILayout.Box("attack: " + State.GetType());
            GUILayout.EndArea();
        }
    }
}