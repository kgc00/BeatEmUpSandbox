using System;
using General;
using Photon.Pun;
using StateMachines.Interfaces;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Models;
using StateMachines.Movement.Vertical.Jumping;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement {
    [RequireComponent(typeof(UnitDataStore))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementFSM : MonoBehaviourPunCallbacks, IAcceptRunInput,
        IAcceptJumpInput, IAcceptDashInput {
        [SerializeField] private JumpConfig jumpConfig;
        [SerializeField] private RunConfig runConfig;
        [SerializeField] private UnitDataStore dataStore;
        public UnitMovementData UnitMovementData { get; private set; }
        [SerializeField] private Rigidbody2D rig;
        public JumpFSM Jump { get; private set; }
        public RunFSM Run { get; private set; }
        private Vector2 relativeForce;
        private Vector2 networkPos;

        private void Start() {
            jumpConfig = jumpConfig.CreateInstance();
            runConfig = runConfig.CreateInstance();
            UnitMovementData = dataStore.store;
            // ReSharper disable twice Unity.InefficientPropertyAccess
            Jump = new JumpFSM(gameObject, jumpConfig, UnitMovementData);
            Run = new RunFSM(gameObject, runConfig, UnitMovementData);
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

        public void RaiseTouchingGroundEvent(bool isTouching) {
            if (!photonView.IsMine) return;

            SetTouchingGround(isTouching);
            photonView.RPC("SetTouchingGround", RpcTarget.Others, isTouching);
        }

        [PunRPC]
        void SetTouchingGround(bool isTouching) {
            if (UnitMovementData == null) return;
            UnitMovementData.touchingGround = isTouching;
        }

        public void RaiseTouchingWallEvent(bool isTouching) {
            if (!photonView.IsMine) return;

            SetTouchingWall(isTouching);
            photonView.RPC("SetTouchingWall", RpcTarget.Others, isTouching);
        }

        [PunRPC]
        void SetTouchingWall(bool isTouching) {
            if (UnitMovementData == null) return;
            UnitMovementData.touchingWall = isTouching;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (!photonView.IsMine) return;

            var id = Helpers.GetIdFromObject(other.gameObject);
            
            CollisionEnter2D_RPC(id);
            photonView.RPC("CollisionEnter2D_RPC", RpcTarget.Others, id);
        }

        [PunRPC]
        private void CollisionEnter2D_RPC(int? id) {
            Jump.OnCollisionEnter2D_RPC(id);
            Run.OnCollisionEnter2D_RPC(id);
        }
    }
}