using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Interfaces;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Models;
using StateMachines.Movement.Vertical.Jumping.States;
using StateMachines.Network;
using StateMachines.Observer;
using StateMachines.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping {
    public class JumpFSM : IProvideForce, IAcceptCollisionEnter, IAcceptJumpInput,
        IAcceptDashInput, IAcceptRunInput,
        IHandleLockedMovementInput, IHandleLockedJumpInput, IOnEventCallback {
        public JumpFS State { get; private set; }
        public JumpConfig Config { get; private set; }
        public UnitMovementData UnitMovementData { get; private set; }
        public GameObject Behaviour { get; private set; }
        public int ViewId { get; private set; }
        public bool IsMine { get; set; }

        public JumpFSM(GameObject behaviour, JumpConfig jumpConfig, UnitMovementData movementData) {
            Config = jumpConfig;
            UnitMovementData = movementData;
            Behaviour = behaviour;
            ViewId = behaviour.GetPhotonView().ViewID;
            IsMine = behaviour.GetPhotonView().IsMine;
            
            InputLockObserver.LockMovementInput += AcceptLockMovementInput;
            InputLockObserver.UnlockMovementInput += AcceptUnlockMovementInput;

            InputLockObserver.LockJumpInput += AcceptLockJumpInput;
            InputLockObserver.UnlockJumpInput += AcceptUnlockJumpInput;

            PhotonNetwork.AddCallbackTarget(this);

            State = new JumpGroundedFS(behaviour, this, jumpConfig);
            State.Enter();
        }


        ~JumpFSM() {
            InputLockObserver.LockMovementInput -= AcceptLockMovementInput;
            InputLockObserver.UnlockMovementInput -= AcceptUnlockMovementInput;

            InputLockObserver.LockJumpInput -= AcceptLockJumpInput;
            InputLockObserver.UnlockJumpInput -= AcceptUnlockJumpInput;

            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void AcceptMoveInput(InputAction.CallbackContext context) => State.AcceptMoveInput(context);

        public void AcceptDashInput(InputAction.CallbackContext context) => State.AcceptDashInput(context);

        public void OnEvent(EventData photonEvent) {
            byte eventCode = photonEvent.Code;

            if (eventCode == NetworkedEventCodes.ChangeJumpStateEventCode) {
                var data = (object[]) photonEvent.CustomData;
                
                if ((int) data[1] != ViewId) return;
                
                var newState = (JumpStates) data[0];

                ChangeState(newState);
            }
            
            
            if (eventCode == NetworkedEventCodes.SetMovementDirEventCode) {
                var data = (object[]) photonEvent.CustomData;
                
                if ((int) data[2] != ViewId) return;

                var dir = (float) data[0];
                var localScale = (Vector3) data[1];
                SetMoveDir(dir, localScale);
            }
        }
        public void RaiseSetMoveDirEvent(float moveDir, Vector3 localScale, int viewId) {
            if (!IsMine) return;
            
            SetMoveDir(moveDir, localScale);
            SetMovementDirEvent.SendSetMovementDirEvent(moveDir, localScale, viewId);
            
        }
        
        public void SetMoveDir(float moveDir, Vector3 localScale) {
            UnitMovementData.moveDir = moveDir;
            Behaviour.transform.localScale = localScale;
        }

        public Vector2 Force() => State.Force();

        public void RaiseChangeStateEvent(JumpStates newState) {
            if (!IsMine) return;
            
            ChangeState(newState);
            ChangeJumpStateEvent.SendChangeJumpStateEvent(newState, ViewId);
            
        }

        public void ChangeState(JumpStates newState) {
            State.Exit();
            State = StateFactory.JumpFSFromEnum(newState, this);
            State.Enter();
        }

        public void Update() => State.Update();
        public void FixedUpdate() => State.FixedUpdate();
        public void OnCollisionEnter2D_RPC() => State.OnCollisionEnter2D_RPC();
        public void AcceptJumpInput(InputAction.CallbackContext context) => State.AcceptJumpInput(context);

        public void AcceptLockMovementInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptLockJumpInput(sender);
        }

        public void AcceptUnlockMovementInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptUnlockJumpInput(sender);
        }

        public void AcceptLockJumpInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptLockJumpInput(sender);
        }

        public void AcceptUnlockJumpInput(object sender) {
            if (!ReferenceEquals(sender, Behaviour)) return;

            State.AcceptUnlockJumpInput(sender);
        }
    }
}