﻿using System;
using Photon.Pun;
using StateMachines.Interfaces;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Models;
using StateMachines.Movement.Vertical.Jumping;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement {
    public class MovementFSM : MonoBehaviourPunCallbacks, IAcceptRunInput, IAcceptJumpInput,
        IAcceptDashInput,
        IPunObservable {
        [SerializeField] private JumpConfig jumpConfig;
        [SerializeField] private RunConfig runConfig;
        [SerializeField] private Rigidbody2D rig;
        public JumpFSM Jump { get; private set; }
        public RunFSM Run { get; private set; }
        private Vector2 relativeForce;

        private void Awake() {
            // ReSharper disable twice Unity.InefficientPropertyAccess
            Jump = new JumpFSM(gameObject, jumpConfig);
            Run = new RunFSM(gameObject, runConfig);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                context.phase != InputActionPhase.Performed &&
                context.phase != InputActionPhase.Canceled) return;
            Run.AcceptMoveInput(context);
            Jump.AcceptMoveInput(context);
        }

        public void AcceptJumpInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                context.phase != InputActionPhase.Performed &&
                context.phase != InputActionPhase.Canceled) return;
            Jump.AcceptJumpInput(context);
        }

        private void Update() {
            Jump.Update();
            Run.Update();
        }

        private void FixedUpdate() {
            Jump.FixedUpdate();
            Run.FixedUpdate();

            relativeForce = Vector2.zero;
            relativeForce += Run.Force();
            relativeForce += Jump.Force();

            rig.AddForce(relativeForce, ForceMode2D.Force);
        }

        public void AcceptDashInput(InputAction.CallbackContext context) {
            if (!photonView.IsMine ||
                context.phase != InputActionPhase.Performed) return;
            Run.AcceptDashInput(context);
            Jump.AcceptDashInput(context);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Board")) return;

            photonView.RPC("CollisionEnter2D_RPC", RpcTarget.All);
        }

        [PunRPC]
        private void CollisionEnter2D_RPC() {
            Jump.OnCollisionEnter2D_RPC();
            Run.OnCollisionEnter2D_RPC();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                // We own this player: send the others our data
                stream.SendNext(relativeForce);
                stream.SendNext(new[] { transform.localScale,transform.position });
            }
            else {
                // Network player, receive data
                relativeForce = (Vector2) stream.ReceiveNext();
                var tr = (Vector3[]) stream.ReceiveNext();
                gameObject.transform.localScale = tr[0];
                gameObject.transform.position = tr[1];
            }
        }

        private void OnGUI() {
            if (!photonView.IsMine) return;

            GUILayout.Box("rig velocity: " + rig.velocity);
            GUILayout.Box("run: " + Run.State.GetType());
            GUILayout.Box("jump: " + Jump.State.GetType());
            GUILayout.Box("jumps left: " + jumpConfig.jumpsLeft);
            GUILayout.Box("air dashes left: " + jumpConfig.dashesLeft);
        }
    }
}